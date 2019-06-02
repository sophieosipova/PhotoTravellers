using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProfileService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly IHostingEnvironment hostingEnvironment;
        public PictureController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        [Route("{url}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        // GET: /<controller>/
        public async Task<ActionResult> GetImage(string url)
        {
            if (url == "")
            {
                return BadRequest();
            }

            try
            {
                var buffer = System.IO.File.ReadAllBytes($"{hostingEnvironment.WebRootPath}/{url}.jpg");
                return File(buffer, "image/jpeg");

            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}