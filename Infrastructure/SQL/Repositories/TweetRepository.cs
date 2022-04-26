using System;
using System.Collections.Generic;
using System.Data;
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
        public async Task<bool> CreateTextTweet(TweetPostModelView model)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == model.UserId);
            var tags = await CreateTagsForTweet(model.Tags);

            if (user != null)
            {
                var tweet = new Tweet()
                {
                    Text = model.TweetText,
                    UserId = model.UserId,
                    Tags = tags,
                    TagCount = tags.Count,
                };

                _context.Add(tweet);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CreatePhotoTweet(CreatePhotoTweetModelDTO model)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == model.UserId);
            if (user != null)
            {
                if (model.Photo != null)
                {
                    var stream = new FileStream(model.Photo.FileName, FileMode.Create);
                    await model.Photo.CopyToAsync(stream);
                    var tweet = new PhotoTweet()
                    {
                        PhotoAddress = stream.Name,
                        UserId = model.UserId
                    };
                    await _context.PhotoTweets.AddAsync(tweet);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<ShowPhotoModel> GetPhotoTweet(int id)
        {
            var tweet = _context.PhotoTweets.FirstOrDefault(x => x.Id == id);
            if (tweet != null && tweet.PhotoAddress != null)
            {
                var tweetWithPhoto = new ShowPhotoModel()
                {
                    Photo = await File.ReadAllBytesAsync(tweet.PhotoAddress)
                };
                return tweetWithPhoto;
            }

            return null;
        }

        public Task<List<Tweet>> MostTaggedTweet()
        {
            var sqlConnection = new SqlConnection(_connectionString);
            var result = sqlConnection.Query<Tweet>("MostTaggedTweet", commandType: CommandType.StoredProcedure)
                .ToList();
            return Task.FromResult<List<Tweet>>(result);
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

        public async Task<SearchTweetByIdModelView> GetTextTweet(int id)
        {
            var tweet = _context.Tweets.Include(x => x.Tags).FirstOrDefault(x => x.Id == id);
            if (tweet != null)
            {
                tweet.TweetViewCount += 1;
                var tagText = new List<string>();
                foreach (var item in tweet.Tags)
                    tagText.Add(item.Word);

                var model = new SearchTweetByIdModelView()
                {
                    TweetText = tweet.Text,
                    Tags = tagText,
                    TweetViewCount = tweet.TweetViewCount,
                    TagCount = tweet.TagCount,
                    UserId = tweet.UserId,
                };
                await _context.SaveChangesAsync();
                return model;
            }
            else
            {
                return null;
            }
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
