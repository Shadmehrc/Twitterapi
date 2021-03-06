using System;
using System.Collections.Generic;
using System.Data;
using System.Formats.Asn1;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces;
using Core;
using Core.Entities;
using Core.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class TweetCrudService : ITweetCrudService
    {
        private readonly ITweetRepository _iTweetRepository;
        private readonly INotificationRepository _notificationRepository;

        public TweetCrudService(ITweetRepository iTweetRepository, IConfiguration configuration,  INotificationRepository notificationRepository)
        {
            _iTweetRepository = iTweetRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task<bool> CreateTextTweet(TweetPostModelView tweetModel)
        {
            var userTaggedList = new List<UserTagged>();
            foreach (var item in tweetModel.TagUsers)
            {
                var userTagged = new UserTagged()
                {
                    UserId = item
                };
                userTaggedList.Add(userTagged);
            }

            var tweet = new Tweet()
            {
                Text = tweetModel.TweetText,
                UserId = tweetModel.UserId,
            };
          
            var model = new CreateTextTweetModel()
            {
                TagsWords = tweetModel.Tags,
                Tweet = tweet,
                UserTaggeds = userTaggedList
            };
            var result = await _iTweetRepository.CreateTextTweet(model);
            if (result == true)
            {
                foreach (var item in model.UserTaggeds)
                {
                    var sendNotif = await _notificationRepository.SendTaggedUserNotification(model.Tweet.UserId, item.UserId);
                }
            }
            return result;
        }

        public async Task<bool> CreatePhotoTweet(CreatePhotoTweetModelDTO model)
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
                var result = await _iTweetRepository.CreatePhotoTweet(tweet);
                return result;
            }

            return false;
        }

        public async Task<bool> DeleteTweet(int id)
        {
            var result = await _iTweetRepository.DeleteTweet(id);

            return result;
        }

        public async Task<bool> EditTweet(int id, string text)
        {
            var result = await _iTweetRepository.EditTweet(id, text);
            return result;
        }


        public async Task<Tweet> FindHelper(int id)
        {
            var result = await _iTweetRepository.FindHelper(id);
            return result;
        }

        public async Task<ShowPhotoModel> GetPhotoTweet(int id)
        {
            var result = await _iTweetRepository.GetPhotoTweet(id);
            var tweetWithPhoto = new ShowPhotoModel()
            {
                Photo = await File.ReadAllBytesAsync(result.PhotoAddress)
            };
            return tweetWithPhoto;
        }

        public async Task<List<ShowMostTaggedTweetModel>> MostTaggedTweet()
        {
            var result = await _iTweetRepository.MostTaggedTweet();
            return result;
        }

        public async Task<List<Tweet>> MostViewedTweet()
        {
            var result = await _iTweetRepository.MostViewedTweet();
            return result;
        }

        public async Task<List<Tweet>> MostLikedTweet()
        {
            var result = await _iTweetRepository.MostLikedTweet();
            return result;
        }

        public async Task<SearchTweetByIdModelView> GetTextTweet(int id)
        {
            var tweet = await _iTweetRepository.GetTextTweet(id);
            var tagText = new List<string>();
            foreach (var item in tweet.Tweet.Tags)
                tagText.Add(item.Word);

            var model = new SearchTweetByIdModelView()
            {
                TweetText = tweet.Tweet.Text,
                Tags = tagText,
                TweetViewCount = tweet.Tweet.TweetViewCount,
                TagCount = tweet.Tweet.TagCount,
                UserId = tweet.Tweet.UserId,
                UserTagged = tweet.UserlistName
            };
            return model;
        }

        public async Task<bool> LikeTweet(int id)
        {
            var result = await _iTweetRepository.LikeTweet(id);
            return result;
        }

        public async Task<bool> Retweet(RetweetModel retweetModel)
        {
            var result = await _iTweetRepository.Retweet(retweetModel);
            return result;
        }

        public async Task<List<string>> FindUserTaggedTweets(string userId)
        {
           var result=  await _iTweetRepository.FindUserTaggedTweets(userId);
           return result;
        }

    }
}
