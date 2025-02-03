using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TrustVault_backend.Services.Interface;

namespace TrustVault_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {
        private readonly IForgotPasswordService _forgotPasswordService;


        public ForgotPasswordController(IForgotPasswordService forgotPasswordService)
        {
            _forgotPasswordService = forgotPasswordService;
        }

        // Step 1: Send OTP to user's email
        [HttpPost("send-otp/{email}")]
        public async Task<IActionResult> SendOtp(string email)
        {
            var result = await _forgotPasswordService.SendOtpAsync(email);
            if (result)
            {
                return Ok(new { message = "OTP sent successfully!" });
            }
            return BadRequest("Email not found or error sending OTP.");
        }

        // Step 2: Verify OTP and reset password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _forgotPasswordService.VerifyOtpAndResetPasswordAsync(request.Email, request.ResetCode, request.NewPassword);
            if (result)
            {
                return Ok(new { message = "Password updated successfully!" });
            }
            return BadRequest("Invalid OTP or error resetting password.");
        }
    }

}
