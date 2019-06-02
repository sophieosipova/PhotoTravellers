using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizationService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedAutorizationOptions;
using SharedModels;

namespace AuthorizationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {

        private readonly ITokenGenerator tokenGenerator;

        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;


        public AuthorizationController(

            ITokenGenerator tokenGenerator,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager
        )
        {

            this.tokenGenerator = tokenGenerator;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;

            if (!roleManager.Roles.Any())
            {
                roleManager.CreateAsync(new IdentityRole("Admin"));
                roleManager.CreateAsync(new IdentityRole("User"));
            }

        }


        [HttpGet, AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<IdentityUser>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsers()
        {

            if (!userManager.Users.Any())
            {
                var user = new IdentityUser { UserName = "Sophie" };
                var result = await userManager.CreateAsync(user, "qwe123456");
                await userManager.AddClaimAsync(user, new Claim("UserName", user.UserName));
                await userManager.AddClaimAsync(user, new Claim("Role", "Admin"));
                var app = new IdentityUser { UserName = "Polina" };
                var result1 = await userManager.CreateAsync(app, "qwe123456");
                await userManager.AddClaimAsync(app, new Claim("userName", app.UserName));
                await userManager.AddClaimAsync(user, new Claim("Role", "User"));

                await userManager.AddToRoleAsync(user, "Admin");
                await userManager.AddToRoleAsync(app, "User");

            }
            return Ok(userManager.Users);
        }

        [HttpPost, AllowAnonymous]
        public async Task<ActionResult<UsersToken>> Login([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(user.UserName, user.Password, false, false);

                if (result.Succeeded)
                {
                    var acccount = await userManager.FindByNameAsync(user.UserName);

                    if (acccount != null)
                    {
                        var jwt = tokenGenerator.GenerateRefreshToken(acccount.Id, acccount.UserName);
                        await userManager.UpdateAsync(acccount);
                        return new UsersToken()
                        {
                            RefreshToken = jwt,
                            AccessToken = tokenGenerator.GenerateAccessToken(acccount.Id, acccount.UserName),
                            UserName = user.UserName,
                            UserId = acccount.Id
                        };

                    }

                }

            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> ValidateAdmin()
        {
            if (this.User.Identity.IsAuthenticated)
            {

                string token = Request.Headers["Authorization"].ToString().Remove(0, 7);
                var user = await userManager.FindByNameAsync(User.Identity.Name/*usersToken.UserName*/);

                if (tokenGenerator.ValidateAccessToken(token))
                {
                    return Ok(true);
                }
            }
            return Unauthorized();
        }



        [HttpGet]
        [Route("user")]
        [Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<bool>> ValidateUser()
        {
            if (this.User.Identity.IsAuthenticated)
            {

                string token = Request.Headers["Authorization"].ToString().Remove(0, 7);
                var user = await userManager.FindByNameAsync(User.Identity.Name/*usersToken.UserName*/);

                if (tokenGenerator.ValidateAccessToken(token))
                {
                    return Ok(true);
                }
            }
            return Unauthorized();
        }


        [HttpGet]
        [Route("logout")]
        public async Task<ActionResult<bool>> Logout()
        {
              await signInManager.SignOutAsync();           
              return Ok(true);
        }

        [HttpGet, Route("refreshtokens")]
        public async Task<ActionResult<UsersToken>> RefreshTokens()
        {
            if (this.User.Identity.IsAuthenticated)
            {

                string token = Request.Headers["Authorization"].ToString().Remove(0, 7);
                var user = await userManager.FindByNameAsync(User.Identity.Name/*usersToken.UserName*/);

                if (tokenGenerator.ValidateRefreshToken(token))
                {
                    var jwt = tokenGenerator.GenerateRefreshToken(user.Id, user.UserName);
                    await userManager.UpdateAsync(user);
                    return new UsersToken()
                    {
                        RefreshToken = jwt,
                        AccessToken = tokenGenerator.GenerateAccessToken(user.Id, user.UserName),
                        UserName = user.UserName,
                        UserId = user.Id
                    };
                }
            }

            return Unauthorized();
        }
   }
}