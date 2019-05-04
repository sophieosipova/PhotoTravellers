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
    public class PostController : ControllerBase
    {
        private readonly IPostsRepository postsRepository;
        public PostController(IPostsRepository postsRepository)
        {
            this.postsRepository = postsRepository;
        }

        [Route("profile/{profileId}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(List<Post>), (int)HttpStatusCode.OK)]
     //   [ProducesResponseType(typeof(PaginatedModel<Post>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersProducts(string profileId)
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
        public async Task<IActionResult> GetUsersProducts(string profileId, [FromQuery]int pageSize, [FromQuery]int pageIndex)
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
    }
}