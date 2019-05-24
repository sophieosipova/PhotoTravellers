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

            TokenGenerator tokenGenerator,
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
                await userManager.AddClaimAsync(user, new Claim("userName", user.UserName));
                var app = new IdentityUser { UserName = "Polina" };
                var result1 = await userManager.CreateAsync(app, "qwe123456");
                await userManager.AddClaimAsync(app, new Claim("userName", app.UserName));
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

    }
}