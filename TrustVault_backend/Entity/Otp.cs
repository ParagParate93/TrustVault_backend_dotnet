using System.ComponentModel.DataAnnotations;

namespace TrustVault_backend.Entity
{
    public class Otp
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string OtpCode { get; set; }

        [Required]
        public string Email { get; set; }

        public string Role { get; set; }

        [Required]
        public DateTime GeneratedOn { get; set; }

        [Required]
        public DateTime ExpiredOn { get; set; }
    }
}
