using System;
using System.Collections.Generic;
using System.Data;
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

        public TweetCrudService(ITweetRepository iTweetRepository, IConfiguration configuration)
        {
            _iTweetRepository = iTweetRepository;
        }

        public async Task<bool> CreateTextTweet(TweetPostModelView model)
        {
            var result = await _iTweetRepository.CreateTextTweet(model);
            return result;
        }

        public async Task<bool> CreatePhotoTweet(CreatePhotoTweetModelDTO model)
        {
            var result = await _iTweetRepository.CreatePhotoTweet(model);
            return result;
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

        public Task<ShowPhotoModel> GetPhotoTweet(int id)
        {
            var result = _iTweetRepository.GetPhotoTweet(id);
            return result;
        }

        public async Task<List<Tweet>> MostTaggedTweet()
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
            var result = await _iTweetRepository.GetTextTweet(id);
            return result;
        }

        public async Task<bool> LikeTweet(int id)
        {
            var result = await _iTweetRepository.LikeTweet(id);
            return result;
        }
    }
}
