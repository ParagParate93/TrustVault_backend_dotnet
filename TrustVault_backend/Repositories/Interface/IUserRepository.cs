using TrustVault_backend.DTO;
using TrustVault_backend.Entity;

namespace TrustVault_backend.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<User> UpdateUserByAdminAsync(User user);
    }
}
