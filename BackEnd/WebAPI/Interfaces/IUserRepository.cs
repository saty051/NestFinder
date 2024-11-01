using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Authenticate(string userName, string password);
        void Register(string username, string password, string email, string phoneNumber, SecurityQuestion securityQuestion, string securityAnswer);
        Task<bool> UserAlreadyExists(string userName);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserContactInfoByIdAsync(int userId);
        void UpdateUser(User user);
    }
}
