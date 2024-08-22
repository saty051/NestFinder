using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Repo
{
    public class CityRepository : ICityRepository
    {
        private readonly DataContext _dataContext;

        public CityRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public void AddCity(City city)
        {
            _dataContext.Cities.Add(city);
        }

        public void DeleteCity(int cityId)
        {
            var city = _dataContext.Cities.Find(cityId);
            _dataContext.Cities.Remove(city);
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _dataContext.Cities.ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
