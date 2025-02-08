using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrustVault_backend.Entity
{
    public class User
    {
       
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[#@$*]).{5,20}$", ErrorMessage = "Password must be 5-20 characters long and include at least one digit, one lowercase letter, and one special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string Phone { get; set; }

        public string Role { get; set; }

        [StringLength(500, ErrorMessage = "Bio cannot be longer than 500 characters")]
        public string? Bio { get; set; }

        public string? ProfilePicture { get; set; }
    }
}
