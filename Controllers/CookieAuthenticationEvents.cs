using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.Cookies;
using shipping_service.Services;

namespace shipping_service.Controllers
{
    public class CookieAuthenticationEvents : Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
    {
        private IAccountService _accountService;
        public CookieAuthenticationEvents(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            // Validate the Principal
            var userPrincipal = context.Principal;

            // If not valid, Sign out
            if (!await _accountService.Exists(userPrincipal.Identity.Name))
            {
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}
