using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Dtos;
using WebAPI.Errors;
using WebAPI.Extensions;
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

            if (user == null)
            {
                _logger.LogWarning("Login failed for user {Username}: Invalid username or password", loginReq.Username);

                ApiError apiError = new ApiError
                {
                    ErrorCode = Unauthorized().StatusCode,
                    ErrorMessage = "Invalid User Name or Password",
                    ErrorDetails = "This error occurs when provided user name or password does not exist"
                };
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
        public async Task<IActionResult> Register(LoginReqDto loginReq)
        {
            _logger.LogInformation("Register attempt for user {Username}", loginReq.Username);

            ApiError apiError = new ApiError();

            // Validate empty fields
            if (loginReq.Username.IsEmpty() || loginReq.Password.IsEmpty() ||
                loginReq.Email.IsEmpty() || loginReq.TelegramId.IsEmpty() || loginReq.PhoneNumber.IsEmpty())
            {
                _logger.LogWarning("Registration failed: Fields cannot be empty for user {Username}", loginReq.Username);

                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Fields cannot be empty.";
                return BadRequest(apiError);
            }

            // Check if user already exists
            if (await _uow.UserRepository.UserAlreadyExists(loginReq.Username))
            {
                _logger.LogWarning("Registration failed: User already exists for username {Username}", loginReq.Username);

                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "User already exists, please login using the same credentials";
                return BadRequest(apiError);
            }

            // Register the user with new fields (including Email, PhoneNumber, and TelegramId)
            _uow.UserRepository.Register(loginReq.Username, loginReq.Password, loginReq.Email, loginReq.PhoneNumber, loginReq.TelegramId);
            await _uow.SaveAsync();

            _logger.LogInformation("User {Username} registered successfully", loginReq.Username);

            return StatusCode(201);
        }

        private string CreateJWT(User user)
        {
            _logger.LogInformation("Generating JWT for user {Username}", user.Username);

            var secretKey = _configuration.GetSection("AppSettings:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),                // Include Email
                new Claim("PhoneNumber", user.PhoneNumber),             // Include Phone Number
                new Claim("TelegramId", user.TelegramId)                // Include Telegram ID
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
