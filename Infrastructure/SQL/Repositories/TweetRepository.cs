using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Application.RepositoryInterfaces;
using Core;
using Core.Entities;
using Core.Models;
using Dapper;
using Infrastructure.SQL.Context;
using Mapster;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.SQL.Repositories
{
    public class TweetRepository : ITweetRepository
    {
        private readonly DatabaseContext _context;
        private readonly string _connectionString;

        public TweetRepository(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _context = databaseContext;
            _connectionString = configuration["connection"];
        }
        public async Task<Tweet> FindHelper(int id)
        {
            var result = _context.Tweets.FirstOrDefault(x => x.Id == id);
            if (result != null)
            {
                result.TweetViewCount += 1;
                await _context.SaveChangesAsync();
            }
            return result;
        }
        public async Task<bool> Retweet(RetweetModel retweetModel)
        {
            var tweet = _context.Tweets.FirstOrDefault(x => x.Id == retweetModel.TweetId);
            var user = _context.Users.FirstOrDefault(x => x.Id == retweetModel.UserId);
            if (tweet != null && user != null)
            {
                var newTweet = new Tweet()
                {
                    TagCount = tweet.TagCount,
                    Likes = tweet.Likes,
                    Tags = tweet.Tags,
                    TweetViewCount = tweet.TweetViewCount,
                    Text = tweet.Text,
                    UserId = tweet.UserId,
                    User = user,
                };
                var result = await CreateTagsForTweet(retweetModel.tagRetweet);
                newTweet.Tags.AddRange(result);
                _context.Add(newTweet);
                await _context.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }
        public Task<List<string>> FindUserTaggedTweets(string userId)
        {
            var sqlConnection = new SqlConnection(_connectionString);
            var result = sqlConnection.Query<string>("FindUserTaggedTweets", new { TweetId = userId },
                commandType: CommandType.StoredProcedure).ToList();
            return Task.FromResult(result);
        }
        public async Task<List<TweetTags>> CreateTagsForTweet(List<string> model)
        {
            var tags = new List<Tag>();
            var tweetTags = new List<TweetTags>();
            if (model != null)
            {
                var sqlConnection = new SqlConnection(_connectionString);
                foreach (var item in model)
                {
                    var tag = await sqlConnection.QueryFirstOrDefaultAsync<Tag>("CreateTag", new { Word = item },
                        commandType: CommandType.StoredProcedure);
                    tags.Add(tag);
                }

                foreach (var item in tags)
                {
                    var tweetTag = new TweetTags()
                    {
                        TagId = item.Id,
                        Word = item.Word
                    };
                    tweetTags.Add(tweetTag);
                }
            }

            return tweetTags;
        }

        public async Task<bool> CreateTextTweet(CreateTextTweetModel model)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == model.Tweet.UserId);
            var tags = await CreateTagsForTweet(model.TagsWords);
            if (user != null)
            {
                model.Tweet.Tags = tags;
                model.Tweet.TagCount = tags.Count;
                model.Tweet.UserTagged = model.UserTaggeds;
                _context.Add(model.Tweet); 
                var rowChangeCount =await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> CreatePhotoTweet(PhotoTweet model)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == model.UserId);
            if (user != null)
            {
                await _context.PhotoTweets.AddAsync(model);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public Task<PhotoTweet> GetPhotoTweet(int id)
        {
            var tweet = _context.PhotoTweets.FirstOrDefault(x => x.Id == id);
            if (tweet != null && tweet.PhotoAddress != null)
            {
                return Task.FromResult(tweet);
            }

            return Task.FromResult<PhotoTweet>(null);
        }
        public Task<List<ShowMostTaggedTweetModel>> MostTaggedTweet()
        {
            var sqlConnection = new SqlConnection(_connectionString);
            var result = sqlConnection.Query<ShowMostTaggedTweetModel>("MostTaggedTweet", commandType: CommandType.StoredProcedure)
                .ToList();
            return Task.FromResult(result);
        }
        public async Task<bool> EditTweet(int id, string text)
        {
            var tweet = await FindHelper(id);
            if (tweet != null)
            {
                tweet.Text = text;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> DeleteTweet(int id)
        {
            var tweet = await FindHelper(id);
            if (tweet != null)
            {
                _context.Tweets.Remove(tweet);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<GetTextTweetModel> GetTextTweet(int id)
        {
            var tweet = _context.Tweets.Include(x => x.Tags).Include(z => z.UserTagged).FirstOrDefault(x => x.Id == id);
            var sqlConnection = new SqlConnection(_connectionString);
            var userList = new List<string>();
            if (tweet != null)
            {
                foreach (var item in tweet.UserTagged)
                {
                    var userTagged = sqlConnection.Query<string>("FindUserFullName", new { id = item.UserId },
                        commandType: CommandType.StoredProcedure);
                    userList.Add(userTagged.First());
                }
            }
            var result = new GetTextTweetModel()
            {
                Tweet = tweet,
                UserlistName = userList
            };
            tweet.TweetViewCount += 1;
            await _context.SaveChangesAsync();
            return result;

        }
        public Task<List<Tweet>> MostViewedTweet()
        {
            var sqlConnection = new SqlConnection(_connectionString);
            var result = sqlConnection.Query<Tweet>("MostViewedTweet", commandType: CommandType.StoredProcedure)
                .ToList();
            return Task.FromResult(result);
        }
        public Task<List<Tweet>> MostLikedTweet()
        {
            var sqlConnection = new SqlConnection(_connectionString);
            var result = sqlConnection.Query<Tweet>("MostLikedTweet", commandType: CommandType.StoredProcedure)
                .ToList();
            return Task.FromResult(result);
        }
        public async Task<bool> LikeTweet(int id)
        {
            var tweet = _context.Tweets.FirstOrDefault(x => x.Id == id);
            if (tweet != null)
            {
                tweet.Likes += 1;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
