using TrustVault_backend.Entity;

namespace TrustVault_backend.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }

        public string Token { get; set; }

        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            Phone = user.Phone;
            Role = user.Role;
            Token = token;
        }
    }
}
