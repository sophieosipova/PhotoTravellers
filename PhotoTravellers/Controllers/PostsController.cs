using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoTravellers.Models;
using PhotoTravellers.Services;
using SharedModels;

namespace PhotoTravellers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostsService postsService;
        public PostsController(IPostsService postsService)
        {
            this.postsService = postsService;
        }


        [HttpGet]
        [Route("profile/{profileId}")]
        [ProducesResponseType(typeof(List<PostModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(PaginatedModel<PostModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersPosts(string profileId, [FromQuery]int pageSize, [FromQuery]int pageIndex)
        {
            if (profileId == "")
                return BadRequest();

            if (pageSize == 0 && pageIndex == 0)
            {
                var posts = await postsService.GetProfilesPosts(profileId);

                if (posts == null)
                    return NotFound();

                return Ok(posts);
            }

            var model = await postsService.GetProfilesPosts(profileId, pageSize, pageIndex);

            if (model == null)
                return NotFound();

            return Ok(model);
        }



        [HttpGet]
        [Route("location/{locationId:int}")]
        [ProducesResponseType(typeof(List<PostModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(PaginatedModel<PostModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPostsFromLocation(int locationId, [FromQuery]int pageSize, [FromQuery]int pageIndex)
        {
            if (locationId <= 0)
                return BadRequest();

            if (pageSize == 0 && pageIndex == 0)
            {
                var posts = await postsService.GetPostsFromLocation(locationId);

                if (posts == null)
                    return NotFound();

                return Ok(posts);
            }

            var model = await postsService.GetPostsFromLocation(locationId, pageSize, pageIndex);

            if (model == null)
                return NotFound();

            return Ok(model);
        }

        [HttpPost]
        [Route("profile/{profileId}")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreatePost(string profileId, PostModel post)
        {
            PostModel p = await postsService.CreatePost(profileId, post);
            if (p == null)
                return Conflict();
            else
                return Created("", p);

        }

        //DELETE api/v1/[controller]/id
        [Route("profile/{profileId}/post/{postId:int}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeletePost(string profileId, int postId)
        {
            if (await postsService.DeletePost(profileId,postId) != null)
                return NotFound();
            else
                return NoContent();
        }

        [HttpPut]
        [Route("profile/{profileId}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> UpdatePost(string profileId, [FromBody] PostModel postToUpdate)
        {
            var post = await postsService.UpdatePost(profileId,postToUpdate);

            if (post == null)
                return NotFound();

            return Created("", post);
        }
    }
}