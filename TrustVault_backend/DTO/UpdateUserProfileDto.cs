using System.ComponentModel.DataAnnotations;

namespace TrustVault_backend.DTO
{
    public class UpdateUserProfileDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        public string Email { get; set; }

        // Password is optional during an update:
        public string? Password { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string Phone { get; set; }

        [StringLength(500, ErrorMessage = "Bio cannot be longer than 500 characters")]
        public string? Bio { get; set; }

        public string? ProfileImage { get; set; }
    }
}
