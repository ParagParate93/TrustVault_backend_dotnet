using Microsoft.AspNetCore.Mvc;
using TrustVault_backend.Entity;
using TrustVault_backend.Models;
using TrustVault_backend.Services.Implementation;
using TrustVault_backend.Services.Interface;

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
            var response = _userService.Authenticate(model);
            if (response == null)
            {
                return BadRequest(new { message = "Invalid email or password." });
            }
            try
            {

                var otpCode = await _otpService.GenerateOtpAsync(response.Email, response.Role); 
                return Ok(new { message = "Authentication successful. OTP has been sent to your email.", otpCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error sending OTP: " + ex.Message });
            }
        }
    }
}
