using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using shipping_service.Models;
using shipping_service.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
namespace shipping_service.Controllers
{
    /// <summary>
    /// Web API to login a authenticated user, and store these claims into a local cookie.
    /// </summary>
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAccountService _accountService;

        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> LoginPost(User user)
        {
            UserLogin userLogin = new();
            userLogin.Username = user.Username;
            userLogin.Password = user.Password;
            var (error, _) = await _accountService.LoginAsync(userLogin);
            if (error == null)
            {

                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.Username));

                if (user.Role == "Sender")
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Sender"));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Courier"));
                }
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                { };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            }
            return this.Ok("/");
        }

        [Authorize]
        [HttpPost]
        //, IgnoreAntiforgeryToken]
        [Route("account/manage")]
        public async Task<ActionResult> UpdatePost(UpdateUser user)
        {
            var username = user.NewUsername;
            var role = user.Role;
                             await _accountService.UpdateDatabaseAsync(username, user.OldUsername, role);
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, username));
            claims.Add(new Claim(ClaimTypes.Role, role));
            var claimsIdentity = new ClaimsIdentity(
    claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            { };
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
