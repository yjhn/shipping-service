using shipping_service.Models;
namespace shipping_service.Services
{
    public interface IAccountService
    {
        Task<string> LoginAsync(User user);
    Task<string> RegisterAsync(User user);
        Task<string> UpdateAsync(User user, string oldUsername);
        Task<string> ChangePasswordAsync(string oldPassword, string newPassword, string username);
    }
}
