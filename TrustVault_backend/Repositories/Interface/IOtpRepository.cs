using TrustVault_backend.Entity;

namespace TrustVault_backend.Repositories.Interface
{
    public interface IOtpRepository
    {
        Task SaveOtpAsync(Otp otp);
        Task<Otp> GetOtpByEmailAsync(string email);
        Task<bool> IsOtpExpiredAsync(long otpId);

        Task TruncateOtpTableAsync();

    }

}
