using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
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

        // api/account/login
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

        // api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(LoginReqDto loginReq)
        {
            _logger.LogInformation("Register attempt for user {Username}", loginReq.Username);

            ApiError apiError = new ApiError();

            // Validate empty username or password
            if (loginReq.Username.IsEmpty() || loginReq.Password.IsEmpty())
            {
                _logger.LogWarning("Registration failed: Username or password cannot be empty for user {Username}", loginReq.Username);

                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Username or password cannot be empty.";
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

            // Register the user
            _uow.UserRepository.Register(loginReq.Username, loginReq.Password);
            await _uow.SaveAsync();

            _logger.LogInformation("User {Username} registered successfully", loginReq.Username);

            return StatusCode(201);
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
