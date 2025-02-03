using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using TrustVault_backend.Entity;
using TrustVault_backend.Helper;
using TrustVault_backend.Models;
using TrustVault_backend.Repositories.Interface;
using TrustVault_backend.Services.Interface;
using BCrypt.Net;


namespace TrustVault_backend.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public AuthenticateResponse? Authenticate(AuthenticateRequest model)
        {
            var user = _userRepository.GetUserByEmailAsync(model.Email).Result;
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return null;
            }
            //var token = "sdfsdf";
            var token = generteJwtToken(user);
            return new AuthenticateResponse(user, token);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<bool> RegisterUserAsync(User user)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);

            if (existingUser != null)
                return false; // User already exists

            // Hash the password before storing it
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            await _userRepository.AddUserAsync(user);
            return true;
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }


        public string generteJwtToken(User user)
        {
            // generate token that is valid for 7 days
            //1. Create a new instance of the JwtSecurityTokenHandler class.
            ////This class is used to create and validate JSON Web Tokens.
            ///

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("7c1a7a3f68b3f4b29b1175c25e6313a5c02fd94b391b73f88eb4ff6079c232d7");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        async Task<User> IUserService.GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }
    }

}