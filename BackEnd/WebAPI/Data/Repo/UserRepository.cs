using Microsoft.EntityFrameworkCore;
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

        public async Task<User> Authenticate(string username, string password)
        {
            return await _dataContext.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
        }
    }
}
