using shipping_service.Models;

using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using shipping_service.Repositories;
using shipping_service.Persistence.Entities;
using Microsoft.JSInterop;
namespace shipping_service.Services
{
    public class AccountService : IAccountService
    {
        private ISenderRepository _senderRepository;
        private ICourierRepository _courierRepository;
        private IHttpContextAccessor _accessor;
        private IJSRuntime _JSRunTime;
        private Sender sender;
        private Courier courier;
        public AccountService(ISenderRepository senderRepository, ICourierRepository courierRepository, IHttpContextAccessor accessor, IJSRuntime jSRunTime)
        {
            _senderRepository = senderRepository;
            _courierRepository = courierRepository;
            _accessor = accessor;
            _JSRunTime = jSRunTime;
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

        private string LoginBegin(string password)
        {
            if (!ExistsCourier() && !ExistsSender())
            {
                return "Invalid username";
            }
            if ((ExistsCourier() && !ValidatePasswordHash(password, courier.HashedPassword)) || (ExistsSender() && !ValidatePasswordHash(password, sender.HashedPassword)))
            {
                return "The password is incorrect";
            }
return null;
            }

        public async Task<(string, string)> LoginAsync(UserLogin user)
        {
            var username = user.Username;
            await SetUser(username);
        var message = LoginBegin(user.Password);
        if (message!= null)
        {
            return (message, null);
        }

            return !ExistsSender()? (null, "Courier") : (null, "Sender");

        }

        public async Task<string> RegisterAsync(User user)
        {
            var role = user.Role;
            var username = user.Username;
            await SetUser(username);
            if (ExistsSender() || ExistsCourier())
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

        public async Task<string> UpdateAsync(string newUsername, string oldUsername)
        {
            await SetUser(newUsername);
            if (ExistsCourier() || ExistsSender())
            {
                return "This username already exists.";
            }
            await SetUser(oldUsername);
            if (!ExistsCourier() && !ExistsSender())
            {
                return "The specified user was not found";
            }

                return null;

        }
        public async Task UpdateDatabaseAsync(string newUsername, string oldUsername, string role)
        {
            if (role == "Courier")
            {
                courier = await _courierRepository.GetByUsername(oldUsername);
                courier.Username = newUsername;
                await _courierRepository.UpdateAsync(courier);
            }
            else
            {
                sender = await _senderRepository.GetByUsername(oldUsername);
                sender.Username = newUsername;
                await _senderRepository.UpdateAsync(sender);
            }
        }

            public async Task<string> ChangePasswordAsync(string oldPassword, string newPassword, string username)
        {
            await SetUser(username);
        var message = LoginBegin(oldPassword);
        if (message != null)
        {
            return message;
        }
        var password = PasswordHash(newPassword);
if (ExistsCourier())
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
public async Task<bool> Exists(string username)
        {
            await SetUser(username);
            return ExistsSender() || ExistsCourier()?true:false;
        }

private bool ExistsSender()
        {
            return sender != null;
        }

        private bool ExistsCourier()
        {
            return courier!= null;
        }

        private async Task SetUser(string username)
        {
            sender = await _senderRepository.GetByUsername(username);
            courier = await _courierRepository.GetByUsername(username);
        }
    }
}
