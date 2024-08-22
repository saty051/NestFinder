using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Repo;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        public readonly ICityRepository _repo;
        public CityController(ICityRepository repo) {
            _repo = repo;
        }


        // GET api/City
        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
            var cities = await _repo.GetCitiesAsync();
            return Ok(cities);
        }

        // Post api/City/add?cityName=Miami
        // Post api/City/add/Los Angeles

        //[HttpPost("Add")]
        //[HttpPost("Add/{cityName}")]
        //public async Task<IActionResult> AddCity(string cityName)
        //{
        //    City city = new City();
        //    city.Name = cityName;
        //    await _dataContext.Cities.AddAsync(city);
        //    await _dataContext.SaveChangesAsync();
        //    return Ok(city);
        //}

        // Post api/City/post/Los Angeles  --Post the data in JSON Format
        [HttpPost("post")]
        public async Task<IActionResult> AddCity(City city)
        {
            _repo.AddCity(city);
            await _repo.SaveAsync();
            return StatusCode(201);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            _repo.DeleteCity(id);
            await _repo.SaveAsync();
            return Ok(id);
        }

    }
}
