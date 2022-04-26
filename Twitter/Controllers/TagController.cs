using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.ServiceInterfaces;
using Core.ApiResponse;
using Core.Models;
using Infrastructure.SQL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;


namespace Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagCrudService _iTagCrudService;
        public TagController(ITagCrudService iTagCrudService)
        {
            _iTagCrudService = iTagCrudService;
        }

        [HttpPost("CreateTag")]
        public async Task<IActionResult> Create(string word)
        {
            var result = await _iTagCrudService.Create(word);

            if (result)
            {
                return Ok(new ApiResponse().Success("Tag successfully created."));
            }
            else
            {
                return Ok(new ApiResponse().FailedToFind("Some error happened."));
            }
        }

        [HttpGet("ShowTag")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _iTagCrudService.Get(id);
            return Ok(result);
        }

        [HttpPut("EditTag")]
        public async Task<IActionResult> Edit(EditTweetDTO model)
        {
            var result = await _iTagCrudService.Edit(model);
            if (result)
            {
                return Ok(new ApiResponse().Success("Tag successfully Edited."));
            }
            else
            {
                return Ok(new ApiResponse().FailedToFind("Tag not found."));
            }
        }

        [HttpDelete("DeleteTag")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _iTagCrudService.Delete(id);
            if (result)
            {
                return Ok(new ApiResponse().Success("Tag successfully deleted."));
            }
            else
            {
                return Ok(new ApiResponse().FailedToFind("Tag not found."));
            }
        }
    }
}
