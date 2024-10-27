using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Authenticate(string userName, string password);
        void Register(string username, string password, string email, string phoneNumber);
        Task<bool> UserAlreadyExists(string userName);
    }
}
