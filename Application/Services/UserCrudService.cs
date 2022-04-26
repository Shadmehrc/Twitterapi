using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces;
using Core.Entities;
using Core.Models;

namespace Application.Services
{
    public class UserCrudService : IUserCrudService
    {
        private readonly IUserRepository _iUserRepository;

        public UserCrudService(IUserRepository iUserRepository)
        {
            _iUserRepository = iUserRepository;
        }

        public async Task<bool> AddClaimToUser(AddClaimToUserModel model)
        {
            var result = await _iUserRepository.AddClaimToUser(model);
            return result;
        }

        public async Task<bool> CreateUser(UserRegisterModel user)
        {
            var result = await _iUserRepository.CreateUser(user);
            return result;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var result = await _iUserRepository.DeleteUser(id);
            return result;
        }

        public async Task<bool> EditUser(UserEditModel userModel)
        {
            var result = await _iUserRepository.EditUser(userModel);
            return result;
        }

        public async Task<User> Find(int id)
        {
            var result = await _iUserRepository.Find(id);
            return result;
        }

        public async Task<User> FindUser(int id)
        {
            var result = await _iUserRepository.FindUser(id);
            return result;
        }
        public async Task<List<string>> SearchUser(string name)
        {
            var result = await _iUserRepository.SearchUser(name);
            return result;
        }

        public async Task<IList<Claim>> UserClaims(string name)
        {
            var result = await _iUserRepository.UserClaims(name);
            return result;
        }
    }
}
