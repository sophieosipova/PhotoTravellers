using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostsService.Database;
using PostsService.Models;
using SharedModels;

namespace PostsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostsRepository postsRepository;
        public PostsController(IPostsRepository postsRepository)
        {
            this.postsRepository = postsRepository;
        }

        [Route("profile/{profileId}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(List<Post>), (int)HttpStatusCode.OK)]
     //   [ProducesResponseType(typeof(PaginatedModel<Post>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersPosts(string profileId)
        {
            if (profileId == "")
                return BadRequest();

            var posts= await postsRepository.GetProfilesPosts(profileId);

            if (posts == null)
                return NotFound();

            return Ok(posts);
        }



        [HttpGet]
        [Route("profile/{profileId}")]
        [ProducesResponseType(typeof(List<Post>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(PaginatedModel<Post>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersPosts(string profileId, [FromQuery]int pageSize, [FromQuery]int pageIndex)
        {
            if (profileId == "")
                return BadRequest();

            if (pageSize == 0 && pageIndex == 0)
            {
                var posts = await postsRepository.GetProfilesPosts(profileId);

                if (posts == null)
                    return NotFound();

                return Ok(posts);
            }

            var model = await postsRepository.GetProfilesPosts(profileId, pageSize, pageIndex);

            if (model == null)
                return NotFound();

            return Ok(model);
        }

        [HttpPost]
        [Route("profile/{profileId}")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreatePost (string userId, Post post)
        {
            Post p = await postsRepository.CreatePost(post);
            if (p == null)
                return Conflict();
            else
                return Created("", p);

        }

        //DELETE api/v1/[controller]/id
        [Route("profile/{profileId}/post/{postId:int}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteProduct(string profileId, int postId)
        {
            if (await postsRepository.DeletePost(postId) != null)
                return NotFound();
            else
                return NoContent();
        }

        [HttpPut]
        [Route("profile/{profileId}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> UpdateProduct(string profileId, [FromBody] Post postToUpdate)
        {
            var post = await postsRepository.UpdatePost(postToUpdate);

            if (post == null)
                return NotFound();

            return Created("", post);
        }

    }
}