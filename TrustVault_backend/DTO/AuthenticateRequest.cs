using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrustVault_backend.Models
{
    public class AuthenticateRequest
    {
        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
