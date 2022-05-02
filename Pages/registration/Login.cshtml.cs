using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using shipping_service.Models;
using shipping_service.Services;

namespace shipping_service.Pages.Registration;
public class LoginModel : PageModel
{
    private IHttpContextAccessor _accessor;
    private IAccountService _accountService;
    public string error;
    public LoginModel(IHttpContextAccessor accessor, IAccountService accountService)
    {
        _accessor = accessor;
        _accountService = accountService;
    }
    [BindProperty]
    public UserLogin Account { get; set; } = new();
    public async Task<IActionResult> OnGetAsync()
    {
        if (_accessor.HttpContext.User.Identity.IsAuthenticated)
        {
            return Redirect("/");
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        User loginUser = new();
        loginUser.Username = Account.Username;
        loginUser.Password = Account.Password;
        error = await _accountService.LoginAsync(loginUser);
        if (!string.IsNullOrEmpty(error))
        {
            return Page();
        }
        return Redirect("/");
    }

}
public class UserLogin
{
    [Required]
            [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    public String Password { get; set; }
    [Required]
    public string Username { get; set; }

}
