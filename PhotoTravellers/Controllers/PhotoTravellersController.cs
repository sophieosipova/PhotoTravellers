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
    public class PhotoTravellersController : ControllerBase
    {
        private readonly IPhotoTravellersService photoTravellersService;
        private readonly IAutorizationService autorizationService;
        public PhotoTravellersController (IPhotoTravellersService photoTravellersService, IAutorizationService autorizationService)
        {
            this.photoTravellersService = photoTravellersService;
            this.autorizationService = autorizationService;
        }

        [HttpGet]
        [Route("profile/{profileId}")]
        [ProducesResponseType(typeof(List<PostModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(PaginatedModel<PostModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersFeedLine(string profileId, [FromQuery]int pageSize, [FromQuery]int pageIndex)
        {
            if (profileId == "")
                return BadRequest();

            if (!await autorizationService.ValidateUserToken(Request.Headers["Authorization"].ToString()))
                return Unauthorized();

            try
            {
                if (pageSize == 0 && pageIndex == 0)
                {
                    var posts = await photoTravellersService.GetFeed(profileId);

                    if (posts == null)
                        return NotFound();

                    return Ok(posts);
                }

                var model = await photoTravellersService.GetFeed(profileId, pageSize, pageIndex);

                if (model == null)
                    return NotFound();

                return Ok(model);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel() { StatusCode = StatusCodes.Status500InternalServerError, Message = "Не удалось подключиться к серверу" });
            }

            
        }

    }
}