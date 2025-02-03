using Microsoft.EntityFrameworkCore;
using TrustVault_backend.DB_Context;
using TrustVault_backend.Entity;
using TrustVault_backend.Repositories.Interface;

namespace TrustVault_backend.Repositories.Implementation
{
    public class OtpRepository : IOtpRepository
    {
        private readonly ApplicationDbContext _context;

        public OtpRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task SaveOtpAsync(Otp otp)
        {
            await _context.Otps.AddAsync(otp);  
            await _context.SaveChangesAsync();
        }

        public async Task<Otp> GetOtpByEmailAsync(string email)
        {
            return await _context.Otps
                .Where(o => o.Email == email)
                .OrderByDescending(o => o.GeneratedOn)  
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsOtpExpiredAsync(long otpId)
        {
            var otpRecord = await _context.Otps.FindAsync(otpId);
            return otpRecord?.ExpiredOn < DateTime.Now;
        }

        public async Task TruncateOtpTableAsync()
        {
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Otps");
        }
    }
}
