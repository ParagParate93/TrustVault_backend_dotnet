namespace TrustVault_backend.Services.Interface
{
    public interface IForgotPasswordService
    {
        Task<bool> SendOtpAsync(string email);
        Task<bool> VerifyOtpAndResetPasswordAsync(string email, string otp, string newPassword);
    }

}
