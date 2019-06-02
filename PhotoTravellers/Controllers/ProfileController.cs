using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoTravellers.Models;
using PhotoTravellers.Services;

namespace PhotoTravellers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService profilesService;

        public ProfileController(IProfileService profilesService)
        {
            this.profilesService = profilesService;
        }

        [HttpGet]
        [Route("{profileId}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProfileModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProfileById(string profileId)
        {
            if (profileId == "")
                return BadRequest();

            var profile = await profilesService.GetProfileById(profileId);

            if (profile != null)
                return Ok(profile);

            return NotFound();
        }

        [HttpDelete]
        [Route("{profileId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProfile(string profileId)
        {
            if (profileId == "")
                return BadRequest();

            var profile = await profilesService.DeleteProfile(profileId);

            if (profile != null)
                return Ok(profile);

            return NotFound();
        }

        [HttpPut]
        [Route("{profileId}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> UpdateProfile(string userId, [FromBody] ProfileModel profileToUpdate)
        {
            var profile = await profilesService.UpdateProfile(profileToUpdate);

            if (profile == null)
                return NotFound();

            return Created("", profile);
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateProfile(ProfileModel profile)
        {
            var p = await profilesService.CreateProfile(profile);

            if (p == null)
                return Conflict();

            return Created("", p);
        }


        [HttpGet]
        [Route("{profileId}/followers")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProfileModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFollowers(string profileId)
        {
            if (profileId == "")
                return BadRequest();

            var followers = await profilesService.GetFollowers(profileId);

            if (followers != null)
                return Ok(followers);

            return NotFound();
        }

        [HttpGet]
        [Route("{profileId}/followings")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProfileModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFollowings(string profileId)
        {
            if (profileId == "")
                return BadRequest();

            var followers = await profilesService.GetFollowings(profileId);

            if (followers != null)
                return Ok(followers);

            return NotFound();
        }

        [HttpDelete]
        [Route("{profileId}/follow/{followId:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteFollow(string profileId, int followId)
        {
            if (followId == 0)
                return BadRequest();

            var follow = await profilesService.DeleteFollow(followId);

            if (follow != null)
                return Ok(follow);

            return NotFound();
        }

        [HttpPost]
        [Route("{profileId}/follow")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateFolow(string  profileId, FollowModel follow)
        {
            var f = await profilesService.CreateFollow(profileId, follow);

            if (f == null)
                return Conflict();

            return Created("", f);
        }
    }
}