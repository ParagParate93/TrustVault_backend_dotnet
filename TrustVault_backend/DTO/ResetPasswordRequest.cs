using System.ComponentModel.DataAnnotations;

namespace TrustVault_backend.Models
{
   
    public class PasswordResetRequest
    {
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        public string Email { get; set; }

        public string ResetCode { get; set; }

        [Required(ErrorMessage = "Password must be supplied")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[#@$*]).{5,20}$", ErrorMessage = "Invalid password format!")]
        public string NewPassword { get; set; }

    }

}
