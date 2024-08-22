using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public CityController(DataContext dataContext) {
            _dataContext = dataContext;
        }


        // GET api/City
        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
            var cities = await _dataContext.Cities.ToListAsync();
            return Ok(cities);
        }

        // Post api/City/add?cityName=Miami
        // Post api/City/add/Los Angeles

        [HttpPost("Add")]
        [HttpPost("Add/{cityName}")]
        public async Task<IActionResult> AddCity(string cityName)
        {
            City city = new City();
            city.Name = cityName;
            await _dataContext.Cities.AddAsync(city);
            await _dataContext.SaveChangesAsync();
            return Ok(city);
        }

        // Post api/City/post/Los Angeles  --Post the data in JSON Format
        [HttpPost("post")]
        public async Task<IActionResult> AddCity(City city)
        {
            //City city = new City();
            //city.Name = cityName;
            await _dataContext.Cities.AddAsync(city);
            await _dataContext.SaveChangesAsync();
            return Ok(city);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            var city = await _dataContext.Cities.FindAsync(id);
            _dataContext.Cities.Remove(city);
            await _dataContext.SaveChangesAsync();
            return Ok(city);
        }

    }
}
