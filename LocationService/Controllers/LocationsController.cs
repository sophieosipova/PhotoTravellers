using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LocationService.Database;
using LocationService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationsRepository locationsRepository;
        public LocationsController(ILocationsRepository locationsRepository)
        {
            this.locationsRepository = locationsRepository;
        }

        [Route("city/{cityName}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(List<Location>), (int)HttpStatusCode.OK)]
        //   [ProducesResponseType(typeof(PaginatedModel<Post>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCityLocations (string cityName)
        {
            if (cityName == "")
                return BadRequest();

            var locations = await locationsRepository.GetCityLocations(cityName);

            if (locations == null)
                return NotFound();

            return Ok(locations);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateLocation(Location location)
        {
            Location l = await locationsRepository.CreateLocation(location);
            if (l == null)
                return Conflict();
            else
                return Created("", l);

        }

        //DELETE api/v1/[controller]/id
        [Route("location/{locationId:int}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteLocation(int locationId)
        {
            var location = await locationsRepository.DeleteLocation(locationId);
            if (location != null)
                return NotFound();
            else
                return Ok(location);
        }

        [HttpPut]
        [Route("location/{locationId}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> UpdateLocation([FromBody] Location locationToUpdate)
        {
            var location = await locationsRepository.UpdateLocation(locationToUpdate);

            if (location == null)
                return NotFound();

            return Created("", location);
        }
    }
}