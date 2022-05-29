using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Entities;
using Core.Models;

namespace Application.RepositoryInterfaces
{
    public interface ITweetRepository
    {
        public Task<Tweet> FindHelper(int id);
        public Task<bool> CreateTextTweet(CreateTextTweetModel createTextTweetModel);
        public Task<bool> CreatePhotoTweet(PhotoTweet model);
        public Task<bool> EditTweet(int id, string text);
        public Task<bool> DeleteTweet(int id);
        public Task<Tweet> GetTextTweet(int id);
        public Task<PhotoTweet> GetPhotoTweet(int id);
        public Task<List<ShowMostTaggedTweetModel>> MostTaggedTweet();
        public Task<List<Tweet>> MostViewedTweet();
        public Task<List<Tweet>> MostLikedTweet();
        public Task<bool> LikeTweet(int id);
        public Task<bool> Retweet(RetweetModel retweetModel);
        public Task<List<TweetTags>> CreateTagsForTweet(List<string> tags);
    }
}
