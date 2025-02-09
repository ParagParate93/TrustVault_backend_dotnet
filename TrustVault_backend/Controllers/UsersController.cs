using Microsoft.AspNetCore.Mvc;
using TrustVault_backend.Entity;
using TrustVault_backend.Models;
using TrustVault_backend.Services.Implementation;
using TrustVault_backend.Services.Interface;
using TrustVault_backend.DTO;

namespace TrustVault_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreateController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOtpService _otpService;

        public CreateController(IUserService userService, IOtpService otpService)
        {
            _userService = userService;
            _otpService = otpService;
        }

        [HttpPost]  
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");

            var result = await _userService.RegisterUserAsync(user);

            if (!result)
                return BadRequest("User already exists or invalid data");

            return Ok(new { message = "Registration successful" });
        }
        [Helper.Authorize]
        [HttpGet("getalluser")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();

                if (users == null || !users.Any())
                {
                    return NotFound(new { message = "No users found." });
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log the error
                return StatusCode(500, new { message = "An error occurred while retrieving users.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }
            return Ok(user);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id,[FromBody] User user)
        {
            Console.WriteLine($"Received User: {user.Name}, {user.Email}, {user.Role}, {user.Id}");

            var existingUser = await _userService.GetUserByIdAsync(user.Id);
            if (existingUser == null)
            {
                return BadRequest(new { message = $"User with id {user.Id} not found." });
            }
            if (ModelState.IsValid)
            {
                await _userService.UpdateUserAsync(user);
                return Ok(user);
            }
            return BadRequest("Invalid data");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return BadRequest(new { message = $"User with id {id} not found." });
            }
            await _userService.DeleteUserAsync(id);
            return Ok(new { message = "User deleted successfully." });
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> ValidateAsync(AuthenticateRequest model) 
        {
            var response1 = _userService.Authenticate(model);
            if (response1 == null)
            {
                return BadRequest(new { message = "Invalid email or password." });
            }
            try
            {

                var otpCode = await _otpService.GenerateOtpAsync(response1.Email, response1.Role); 
                return Ok(new { message = "Authentication successful. OTP has been sent to your email.", otpCode, response1 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error sending OTP: " + ex.Message });
            }
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile([FromQuery] string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(user);
        }

        [HttpPut("profile/update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Retrieve the existing user using the email from the DTO.
                var existingUser = await _userService.GetUserByEmailAsync(updateDto.Email);
                if (existingUser == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                // Map the DTO properties to the existing user.
                //existingUser.Name = updateDto.Name;
                //existingUser.Phone = updateDto.Phone;
                //existingUser.Bio = updateDto.Bio;
                //existingUser.ProfileImage = updateDto.ProfileImage;
                //existingUser.Password = updateDto.Password;

                // Update the password only if a new one is provided.
                //if (!string.IsNullOrEmpty(updateDto.Password))
                //{
                //    existingUser.Password = updateDto.Password;
                //}

                await _userService.UpdateUserProfileAsync(updateDto);
                return Ok(new { message = "Profile updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
