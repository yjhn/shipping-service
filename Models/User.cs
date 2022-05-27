using System.ComponentModel.DataAnnotations;
namespace shipping_service.Models
{
    public class User
    {
        [Required]
        [RegularExpression(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=_]).*$", ErrorMessage = "Password must be at least 8 characters long, contain at least 1 number, small/capital letter and special symbol")]
        public String Password { get; set; }
        [Required]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string Role { get; set; }
    }

}
