using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using shipping_service.Models;
using shipping_service.Services;

namespace shipping_service.Controllers
{
    /// <summary>
    ///     Web API to login a authenticated user, and store these claims into a local cookie.
    /// </summary>
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> LoginPost(UserLogin user)
        {
            (string? error, string? role) = await _accountService.LoginAsync(user);
            if (error == null)
            {
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    role == "Sender"
                        ? new Claim(ClaimTypes.Role, "Sender")
                        : new Claim(ClaimTypes.Role, "Courier")
                };

                ClaimsIdentity claimsIdentity = new(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties authProperties = new();

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);
            }

            return Ok("/");
        }

        [Authorize]
        [HttpPost]
        [Route("account/manage")]
        public async Task<ActionResult> UpdatePost(UpdateUser user)
        {
            string username = user.NewUsername;
            string role = user.Role;
            await _accountService.UpdateDatabaseAsync(username, user.OldUsername, role);
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, username), 
                new Claim(ClaimTypes.Role, role)
            };
            ClaimsIdentity claimsIdentity = new(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties authProperties = new AuthenticationProperties();
            await HttpContext
                .SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return Ok();
        }
    }

    public class UpdateUser
    {
        public string NewUsername { get; set; }
        public string OldUsername { get; set; }
        public string Role { get; set; }
    }
}
