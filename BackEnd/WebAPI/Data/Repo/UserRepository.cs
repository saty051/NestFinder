using Microsoft.AspNetCore.Identity;
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
        public UserRepository(DataContext datacontext)
        {
                _dataContext = datacontext;
        }

        public async Task<User> Authenticate(string username, string passwordText)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Username == username); //&& x.Password == password
            if(user == null || user.PasswordKey == null)
                return null;
           
            if (!MatchPasswordHash(passwordText, user.Password, user.PasswordKey))
                return null;

            return user;
        }

        private bool MatchPasswordHash(string passwordText, byte[] password, byte[] passwordKey)
        {
            using (var hmac = new HMACSHA512(passwordKey))
            { 
               var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordText));
                for (int i = 0; i < passwordHash.Length; i++)
                {
                    if (passwordHash[i] != password[i])
                        return false;
                }
                return true;
            }
        }

        public  void Register(string userName, string password)
        {
            byte[] passwordHash, passwordKey;

            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

            User user = new User();
            user.Username = userName;
            user.Password = passwordHash;
            user.PasswordKey = passwordKey;

           _dataContext.Users.Add(user);
        }

        public async Task<bool> UserAlreadyExists(string userName)
        {
            return await _dataContext.Users.AnyAsync(x => x.Username == userName);
        }
    }
}
