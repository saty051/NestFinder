using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebAPI.Dtos;
using WebAPI.Errors;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IUnitOfWork uow, IConfiguration configuration, ILogger<AccountController> logger)
        {
            _uow = uow;
            _configuration = configuration;
            _logger = logger;
        }

        // api/Account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginReqDto loginReq)
        {
            _logger.LogInformation("Login attempt for user {Username}", loginReq.Username);

            var user = await _uow.UserRepository.Authenticate(loginReq.Username, loginReq.Password);

            ApiError apiError = new ApiError();

            if (user == null)
            {
                _logger.LogWarning("Login failed for user {Username}: Invalid username or password", loginReq.Username);

                apiError.ErrorCode = Unauthorized().StatusCode;
                apiError.ErrorMessage = "Invalid User Name or Password";
                apiError.ErrorDetails = "This error occurs when provided user name or password does not exist";
                return Unauthorized(apiError);
            }

            _logger.LogInformation("Login successful for user {Username}", user.Username);

            var loginRes = new LoginResDto
            {
                UserName = user.Username,
                Token = CreateJWT(user)
            };

            return Ok(loginRes);
        }

        // api/Account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            _logger.LogInformation("Register attempt for user {Username}", registerDto.Username);

            ApiError apiError = new ApiError();

            // Validate empty username or password
            if (string.IsNullOrEmpty(registerDto.Username) || string.IsNullOrEmpty(registerDto.Password))
            {
                _logger.LogWarning("Registration failed: Username or password cannot be empty for user {Username}", registerDto.Username);

                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Username or password cannot be empty.";
                return BadRequest(apiError);
            }

            // Check if user already exists
            if (await _uow.UserRepository.UserAlreadyExists(registerDto.Username))
            {
                _logger.LogWarning("Registration failed: User already exists for username {Username}", registerDto.Username);

                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "User already exists, please login using the same credentials";
                return BadRequest(apiError);
            }

            // Register the user
            _uow.UserRepository.Register(registerDto.Username, registerDto.Password, registerDto.Email, registerDto.PhoneNumber, registerDto.SecurityQuestion, registerDto.SecurityAnswer);
            await _uow.SaveAsync();

            _logger.LogInformation("User {Username} registered successfully", registerDto.Username);

            return StatusCode(201);
        }

        // api/Account/password-regenerate
        [HttpPost("password-regenerate")]
        public async Task<IActionResult> PasswordRegenerate([FromBody] PasswordRegenerationDto passwordRegenerationDto)
        {
            _logger.LogInformation("Password regeneration attempt for user {Username}", passwordRegenerationDto.Username);

            var user = await _uow.UserRepository.GetUserByUsernameAsync(passwordRegenerationDto.Username);

            if (user == null)
            {
                _logger.LogWarning("Password regeneration failed: User not found for username {Username}", passwordRegenerationDto.Username);

                var apiError = new ApiError
                {
                    ErrorCode = NotFound().StatusCode,
                    ErrorMessage = "User not found",
                    ErrorDetails = "The provided username does not exist."
                };

                return NotFound(apiError);
            }

            if (user.SecurityQuestion != passwordRegenerationDto.SecurityQuestion ||
                user.SecurityAnswer != passwordRegenerationDto.SecurityAnswer)
            {
                _logger.LogWarning("Security question/answer mismatch for user {Username}", passwordRegenerationDto.Username);

                var apiError = new ApiError
                {
                    ErrorCode = BadRequest().StatusCode,
                    ErrorMessage = "Security question or answer is incorrect",
                    ErrorDetails = "The security question or answer provided does not match our records."
                };

                return BadRequest(apiError);
            }

            // Generate new password hash
            byte[] passwordHash, passwordKey;
            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordRegenerationDto.NewPassword));
            }

            user.Password = passwordHash;
            user.PasswordKey = passwordKey;

            _uow.UserRepository.UpdateUser(user);
            var result = await _uow.SaveAsync();

            if (result)
            {
                _logger.LogInformation("Password reset successfully for user {Username}", user.Username);
                return Ok(new { Message = "Password has been reset successfully." });
            }
            else
            {
                _logger.LogError("Error updating password for user {Username}", user.Username);
                return StatusCode(500, "Error updating password. Please try again later.");
            }
        }

        private string CreateJWT(User user)
        {
            _logger.LogInformation("Generating JWT for user {Username}", user.Username);

            var secretKey = _configuration.GetSection("AppSettings:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = signingCredentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            _logger.LogInformation("JWT generated successfully for user {Username}", user.Username);

            return tokenHandler.WriteToken(token);
        }
    }
}
