using shipping_service.Models;

namespace shipping_service.Services
{
    public interface IAccountService
    {
        Task<(string, string)> LoginAsync(UserLogin user);
        Task<string> RegisterAsync(User user);
        Task<string> UpdateAsync(string newUsername, string oldUsername);
        Task<string> ChangePasswordAsync(string oldPassword, string newPassword, string username);
        Task<bool> Exists(string username);
        Task UpdateDatabaseAsync(string newUsername, string oldUsername, string role);
    }
}
