using TrustVault_backend.Entity;
using TrustVault_backend.Repositories.Interface;
using TrustVault_backend.Services.Interface;

namespace TrustVault_backend.Services.Implementation
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOtpRepository _otpRepository;
        private readonly IEmailService _emailService;

        public ForgotPasswordService(IUserRepository userRepository, IOtpRepository otpRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _otpRepository = otpRepository;
            _emailService = emailService;
        }

        // Generate and send OTP to user's email
        public async Task<bool> SendOtpAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var otp = GenerateOtp();
            var otpValidUntil = DateTime.Now.AddMinutes(10); // OTP valid for 10 minutes
            var otpRecord = new Otp
            {
                Email = email,
                OtpCode = otp,
                Role = user.Role,
                GeneratedOn = DateTime.Now,
                ExpiredOn = DateTime.Now.AddMinutes(5)
            };

          
            await _otpRepository.SaveOtpAsync(otpRecord);

            await _emailService.SendEmailAsync(user.Email, "Your OTP for password reset", $"Your OTP is: {otp}");

            return true;
        }

        
        public async Task<bool> VerifyOtpAndResetPasswordAsync(string email, string otp, string newPassword)
        {
            var otpRecord = await _otpRepository.GetOtpByEmailAsync(email);
            if (otpRecord == null || otpRecord.OtpCode != otp || otpRecord.ExpiredOn < DateTime.Now)
            {
                return false; 
            }

           
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user != null)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword); 
                user.Role = user.Role;
                await _userRepository.UpdateUserAsync(user);
                return true;
            }

            return false;
        }

        
        private string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }

}
