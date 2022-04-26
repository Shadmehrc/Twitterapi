using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.ServiceInterfaces;
using Core.ApiResponse;
using Core.Models;
using Infrastructure.SQL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserCrudService _iUserCrudService;
        public UserController(IUserCrudService iUserCrudService)
        {
            _iUserCrudService = iUserCrudService;
        }


        [HttpPost("CreateUser")]
        public async Task<IActionResult> Create([FromQuery] UserRegisterModel user)
        {
            var result = await _iUserCrudService.CreateUser(user);
            if (result)
            {
                return Ok(new ApiResponse().Success(result));
            }
            else
            {
                return Ok(new ApiResponse().FailedToFind("User already exists"));
            }
        }


        [HttpGet("/ShowUserTweets")]
        public async Task<IActionResult> Search(string id)
        {
            var user = await _iUserCrudService.SearchUser(id);
            return Ok(user);
        }
        [HttpPost("AddClaimToUser")]
        public async Task<IActionResult> AddClaim([FromQuery] AddClaimToUserModel model)
        {
            var result = await _iUserCrudService.AddClaimToUser(model);
            if (result)
            {
                return Ok(new ApiResponse().Success("Claim sucessfully added to user."));
            }
            else
            {
                return Ok(new ApiResponse().FailedToFind("User not found."));
            }
            return Ok();
        }

        [HttpGet("UserClaims")]
        public async Task<IActionResult> UserClaims(string name)
        {
            var result = await _iUserCrudService.UserClaims(name);
            return Ok(result);
        }
        [HttpPut("EditUser")]
        public async Task<IActionResult> Edit([FromQuery] UserEditModel user)
        {
            var result = await _iUserCrudService.EditUser(user);
            if (result)
            {
                return Ok(new ApiResponse().Success("User successfully edited."));
            }
            else
            {
                return Ok(new ApiResponse().FailedToFind("User not found."));
            }
        }
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _iUserCrudService.DeleteUser(id);
            if (result)
            {
                return Ok(new ApiResponse().Success("User successfully deleted."));
            }
            else
            {
                return Ok(new ApiResponse().FailedToFind("User not found."));
            }
        }

    }
}
