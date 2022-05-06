using shipping_service.Models;

using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using shipping_service.Repositories;
using shipping_service.Persistence.Entities;
namespace shipping_service.Services
{
    public class AccountService : IAccountService
    {
        private ISenderRepository _senderRepository;
        private ICourierRepository _courierRepository;
        private IHttpContextAccessor _accessor;

        public AccountService(ISenderRepository senderRepository, ICourierRepository courierRepository, IHttpContextAccessor accessor)
        {
            _senderRepository = senderRepository;
            _courierRepository = courierRepository;
            _accessor = accessor;
        }
        private byte[] PasswordHash(string password)
        {
            byte[] salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return hashBytes;
        }
        private bool ValidatePasswordHash(string password, byte[] dbPasswordHash)
        {
            byte[] salt = new byte[16];
            Array.Copy(dbPasswordHash, 0, salt, 0, 16);

            var userPasswordBytes = new Rfc2898DeriveBytes(password, salt, 1000);
            byte[] userPasswordHash = userPasswordBytes.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (dbPasswordHash[i + 16] != userPasswordHash[i])
                {
                    return false;
                }
            }
            return true;
        }

        private string LoginBegin(string password, Courier courier, Sender sender)
        {
            if (courier == null && sender == null)
            {
                return "Invalid username";
            }
            if ((courier != null && !ValidatePasswordHash(password, courier.HashedPassword)) || (sender != null && !ValidatePasswordHash(password, sender.HashedPassword)))
            {
                return "The password is incorrect";
            }
return null;
            }

        public async Task<string> LoginAsync(User user)
        {
            var username = user.Username;
                    var courier = await _courierRepository.GetByUsername(username);
        var sender = await _senderRepository.GetByUsername(username);
        var message = LoginBegin(user.Password, courier, sender);
        if (message!= null)
        {
            return message;
        }
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, username));

            if (sender != null)
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
            await _accessor.HttpContext.SignInAsync(
       CookieAuthenticationDefaults.AuthenticationScheme,
       new ClaimsPrincipal(claimsIdentity),
       authProperties);
            return null;

        }

        public async Task<string> RegisterAsync(User user)
        {
            var role = user.Role;
            var username = user.Username;
            var courier = await _courierRepository.GetByUsername(username);
            var sender = await _senderRepository.GetByUsername(username);
            if (sender != null || courier != null)
            {
                return "This username already exists.";
            }
            var password = PasswordHash(user.Password);
            if (role == "Courier")
            {
                var newCourier = new Courier();
                newCourier.Username = username;
                newCourier.HashedPassword = password;
                await _courierRepository.CreateAsync(newCourier);
            }
            else
            {
                var newSender = new Sender();
                newSender.Username = username;
                newSender.HashedPassword = password;
                await _senderRepository.CreateAsync(newSender);
            }
            return null;
        }
        public async Task<string> UpdateAsync(User user, string oldUsername)
        {
            var username = user.Username;
            var role = user.Role;
            var courier = await _courierRepository.GetByUsername(oldUsername);
            var sender = await _senderRepository.GetByUsername(oldUsername);
            if (sender == null && courier == null)
            {
                return "The specified user was not found";
            }
            var courierExists = await _courierRepository.GetByUsername(username);
            var senderExists = await _senderRepository.GetByUsername(username);
            if (courierExists != null || senderExists != null)
            {
                return "This username already exists.";
            }
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, username));

            if (courier != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Sender"));
                courier.Username = username;
                await _courierRepository.UpdateAsync(courier);
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "Courier"));
                sender.Username = username;
                await _senderRepository.UpdateAsync(sender);
            }
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            { };
/*            await _accessor.HttpContext
            .SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _accessor.HttpContext.SignInAsync(
       CookieAuthenticationDefaults.AuthenticationScheme,
       new ClaimsPrincipal(claimsIdentity),
       authProperties);
*/

            return null;

        }
        public async Task<string> ChangePasswordAsync(string oldPassword, string newPassword, string username)
        {
        var courier = await _courierRepository.GetByUsername(username);
        var sender = await _senderRepository.GetByUsername(username);
        var message = LoginBegin(oldPassword, courier, sender);
        if (message != null)
        {
            return message;
        }
        var password = PasswordHash(newPassword);
if (courier != null)
        {
            courier.HashedPassword = password;
            await _courierRepository.UpdateAsync(courier);
        }
        else
        {
            sender.HashedPassword = password;
            await _senderRepository.UpdateAsync(sender);
        }
        return null;
    }

}
}
