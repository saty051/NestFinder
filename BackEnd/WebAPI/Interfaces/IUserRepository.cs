using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Authenticate(string userName, string password);
        void Register(string userName, string password, string email, string mobile, string telegramId);
        Task<bool> UserAlreadyExists(string userName);
        Task<User> GetUserByTelegramId(string telegramId);
        Task<User> GetUserByEmail(string email);
    }
}
