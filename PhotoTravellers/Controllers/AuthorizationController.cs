using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoTravellers.Services;
using SharedModels;

namespace PhotoTravellers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAutorizationService autorizationService;
        public AuthorizationController (IAutorizationService autorizationService)
        {
            this.autorizationService = autorizationService;
        }

        [HttpPost/*, AllowAnonymous*/]
        public async Task<ActionResult<UsersToken>> Login([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ToString());
            try
            {
                var logged = await autorizationService.Login(user);
                if (logged == null)
                    return BadRequest(new ErrorModel() { StatusCode = StatusCodes.Status400BadRequest, Message = "Не корректные данные" });
                return logged;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel() { StatusCode = StatusCodes.Status500InternalServerError, Message = "Не удалось подключиться к серверу" });
            }

        }

        [HttpPost]
        [Route("refreshtokens")]
        public ActionResult<UsersToken> RefreshTokens(UsersToken usersToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorModel() { StatusCode = StatusCodes.Status400BadRequest, Message = ModelState.ToString() });

            try
            {
                var token = autorizationService.RefreshTokens(usersToken);
                if (token != null)
                    return Ok(token);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel() { StatusCode = StatusCodes.Status500InternalServerError, Message = "Не удалось подключиться к серверу" });
            }

            return BadRequest(new ErrorModel() { StatusCode = StatusCodes.Status400BadRequest, Message = "Не корректные данные" });
        }


        [HttpGet]
        [Route("user")]
        public async Task<ActionResult<bool>> ValidateUser()
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorModel() { StatusCode = StatusCodes.Status400BadRequest, Message = ModelState.ToString() });
            try
            {
                string header = Request.Headers["Authorization"];

                bool flag = await autorizationService.ValidateUserToken(header);
                if (flag)
                    return Ok(flag);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel() { StatusCode = StatusCodes.Status500InternalServerError, Message = "Не удалось подключиться к серверу" });
            }

            return Unauthorized();
        }


        [HttpGet]
        [Route("admin")]
        public async Task<ActionResult<bool>> ValidateAdmin()
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorModel() { StatusCode = StatusCodes.Status400BadRequest, Message = ModelState.ToString() });
            try
            {
                string header = Request.Headers["Authorization"];

                bool flag = await autorizationService.ValidateAdminToken(header);
                if (flag)
                    return Ok(flag);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel() { StatusCode = StatusCodes.Status500InternalServerError, Message = "Не удалось подключиться к серверу" });
            }

            return Unauthorized();
        }
    }
}