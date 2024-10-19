using Microsoft.EntityFrameworkCore;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> Authenticate(string userName, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.Password, user.PasswordKey))
                return null;

            return user;
        }

        public async Task<bool> UserAlreadyExists(string userName)
        {
            return await _context.Users.AnyAsync(u => u.Username == userName);
        }

        public async Task<User> GetUserByTelegramId(string telegramId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.TelegramId == telegramId);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public void Register(string userName, string password, string email, string mobile, string telegramId)
        {
            byte[] passwordHash, passwordKey;
            CreatePasswordHash(password, out passwordHash, out passwordKey);

            var user = new User
            {
                Username = userName,
                Password = passwordHash,
                PasswordKey = passwordKey,
                Email = email,
                PhoneNumber = mobile,
                TelegramId = telegramId
            };

            _context.Users.Add(user);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordKey)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedKey)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedKey))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }
    }
}
