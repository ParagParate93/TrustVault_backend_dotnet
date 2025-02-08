using TrustVault_backend.DTO;
using TrustVault_backend.Entity;
using TrustVault_backend.Models;

namespace TrustVault_backend.Services.Interface
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        Task<User?> UpdateUserByAdminAsync(int id, UpdateUserUsingAdmin updateUserUsingAdmin);
    }
}
