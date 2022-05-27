using System.ComponentModel.DataAnnotations;
namespace shipping_service.Models
{
    public class UserLogin
    {
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public String Password { get; set; }
        [Required]
        public string Username { get; set; }

    }
}