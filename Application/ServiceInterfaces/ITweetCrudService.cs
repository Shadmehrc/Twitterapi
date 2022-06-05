using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Entities;
using Core.Models;

namespace Application.ServiceInterfaces
{
    public interface ITweetCrudService
    {
        public Task<Tweet> FindHelper(int id);
        public Task<bool> CreateTextTweet(TweetPostModelView model);
        public Task<bool> CreatePhotoTweet(CreatePhotoTweetModelDTO model);
        public Task<bool> EditTweet(int id, string text);
        public Task<bool> DeleteTweet(int id);
        public Task<SearchTweetByIdModelView> GetTextTweet(int id);
        public Task<ShowPhotoModel> GetPhotoTweet(int id);
        public Task<List<ShowMostTaggedTweetModel>> MostTaggedTweet();
        public Task<List<Tweet>> MostViewedTweet();
        public Task<List<Tweet>> MostLikedTweet();
        public Task<bool> LikeTweet(int id);
        public Task<bool> Retweet(RetweetModel retweetModel);
        public Task<List<string>> FindUserTaggedTweets(string userId);


    }
}
