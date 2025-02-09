using TrustVault_backend.DB_Context;
using TrustVault_backend.Entity;
using TrustVault_backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace TrustVault_backend.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateUserAsync(User user)
        {            
            
            var existingUser = _context.Users.Local.FirstOrDefault(p => p.Id == user.Id);
            if (existingUser != null)
            {
                _context.Entry(existingUser).State = EntityState.Detached;
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async  Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user != null) {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetAdminsAsync()
        {
            return await _context.Users
                .Where(user => user.Role == "ROLE_ADMIN")
                .ToListAsync();
        }

    }
}


