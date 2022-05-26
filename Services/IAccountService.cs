using shipping_service.Models;

namespace shipping_service.Services
{
    public interface IAccountService
    {
        // Returns (error message, role).
        Task<(string?, string?)> LoginAsync(UserLogin user);

        // Returns error message.
        Task<string?> RegisterAsync(User user);

        // Returns error message.
        Task<string?> UpdateAsync(string newUsername, string oldUsername);

        // Returns error message.
        Task<string?> ChangePasswordAsync(string oldPassword, string newPassword, string username);
        Task<bool> Exists(string username);
        Task UpdateDatabaseAsync(string newUsername, string oldUsername, string role);
    }
}
