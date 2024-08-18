using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;

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



        [HttpGet]
        public IActionResult GetCities()
        {
            var cities = _dataContext.Cities.ToList();
            return Ok(cities);
        }
    }
}
