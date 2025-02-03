namespace TrustVault_backend.Services.Interface
{
    public interface IOtpService
    {
        Task<string> GenerateOtpAsync(string email,string role);
        Task<bool> ValidateOtpAsync(string email, string otp);
        Task<bool> IsOtpExpiredAsync(long otpId);
    }
}
