using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CityController(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }


        // GET api/City
        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
            var cities = await _uow.CityRepository.GetCitiesAsync();
            var citiesDto = _mapper.Map<IEnumerable<CityDto>>(cities);

            return Ok(citiesDto);

        }

        // Post api/City/post/Los Angeles  --Post the data in JSON Format
        [HttpPost("post")]
        public async Task<IActionResult> AddCity(CityDto cityDto)
        {
            var city = _mapper.Map<City>(cityDto);
            city.LastUpdatedOn = DateTime.Now;
            city.LatestUpdatedBy = 1;

            _uow.CityRepository.AddCity(city);
            await _uow.SaveAsync();
            return StatusCode(201);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCity(int id,CityDto cityDto)
        {
            var cityFromDb = await _uow.CityRepository.FindCity(id);
            cityFromDb.LastUpdatedOn = DateTime.Now;
            cityFromDb.LatestUpdatedBy = 1;
            _mapper.Map(cityDto, cityFromDb);
            await _uow.SaveAsync();
            return StatusCode(200);
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateCityPatch(int id, JsonPatchDocument<City> cityToPatch)
        {
            var cityFromDb = await _uow.CityRepository.FindCity(id);
            cityFromDb.LastUpdatedOn = DateTime.Now;
            cityFromDb.LatestUpdatedBy = 1;

            cityToPatch.ApplyTo(cityFromDb, ModelState);
            await _uow.SaveAsync();
            return StatusCode(200);
        }

        [HttpPut("updateCityName/{id}")]
        public async Task<IActionResult> UpdateCity(int id, CityUpdateDto cityDto)
        {
            var cityFromDb = await _uow.CityRepository.FindCity(id);
            cityFromDb.LastUpdatedOn = DateTime.Now;
            cityFromDb.LatestUpdatedBy = 1;
            _mapper.Map(cityDto, cityFromDb);
            await _uow.SaveAsync();
            return StatusCode(200);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            _uow.CityRepository.DeleteCity(id);
            await _uow.SaveAsync();
            return Ok(id);
        }

    }
}
