using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Data.Repo
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<User> Authenticate(string username, string passwordText)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (user == null || user.PasswordKey == null)
                return null;

            if (!MatchPasswordHash(passwordText, user.Password, user.PasswordKey))
                return null;

            return user;
        }

        private bool MatchPasswordHash(string passwordText, byte[] password, byte[] passwordKey)
        {
            using (var hmac = new HMACSHA512(passwordKey))
            {
                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordText));
                for (int i = 0; i < passwordHash.Length; i++)
                {
                    if (passwordHash[i] != password[i])
                        return false;
                }
                return true;
            }
        }

        public void Register(string userName, string password, string email, string phoneNumber, SecurityQuestion securityQuestion, string securityAnswer)
        {
            byte[] passwordHash, passwordKey;

            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            User user = new User
            {
                Username = userName,
                Password = passwordHash,
                PasswordKey = passwordKey,
                Email = email,
                PhoneNumber = phoneNumber,
                SecurityQuestion = securityQuestion.ToString(),
                SecurityAnswer = securityAnswer
            };

            _dataContext.Users.Add(user);
        }

        public async Task<bool> UserAlreadyExists(string userName)
        {
            return await _dataContext.Users.AnyAsync(x => x.Username == userName);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _dataContext.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public void UpdateUser(User user)
        {
            _dataContext.Users.Update(user);
        }
    }
}
