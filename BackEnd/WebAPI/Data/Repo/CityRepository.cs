using Microsoft.EntityFrameworkCore;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Data.Repo
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

        public async Task<City> FindCity(int id)
        {
            return await _dataContext.Cities.FindAsync(id);
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _dataContext.Cities.ToListAsync();
        }
    }
}
