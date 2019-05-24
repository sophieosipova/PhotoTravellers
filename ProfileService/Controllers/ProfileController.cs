using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Database;
using ProfileService.Models;

namespace ProfileService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfilesRepository profilesRepository;

        public ProfileController(IProfilesRepository profilesRepository)
        {
            this.profilesRepository = profilesRepository;
        }

        [HttpGet]
        [Route("{profileId}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Profile), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProfileById(string profileId)
        {
            if (profileId == "")
                return BadRequest();

            var profile = await profilesRepository.GetProfileById(profileId);

            if (profile != null)
                return Ok(profile);

            return NotFound();
        }

        [HttpDelete]
        [Route("{profileId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteProfile(string profileId)
        {
            if (profileId == "")
                return BadRequest();

            if (await profilesRepository.DeleteProfile(profileId) != null)
                return NoContent();

            return NotFound();
        }

        [HttpPut]
        [Route("{profileId}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> UpdateProfile(string userId, [FromBody] Profile profileToUpdate)
        {
            var profile = await profilesRepository.UpdateProfile(profileToUpdate);

            if (profile == null)
                return NotFound();

            return Created("", profile);
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateProfile(Profile profile)
        {
            var p = await profilesRepository.CreateProfile(profile);

            if (p == null)
                return Conflict();

            return Created("", p);
        }

    }
}