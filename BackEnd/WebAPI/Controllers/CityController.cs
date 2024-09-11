using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Authorize]
    public class CityController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<CityController> _logger;

        public CityController(IUnitOfWork uow, IMapper mapper, ILogger<CityController> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        // GET api/City
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
            _logger.LogInformation("Fetching all cities.");

            var cities = await _uow.CityRepository.GetCitiesAsync();
            var citiesDto = _mapper.Map<IEnumerable<CityDto>>(cities);

            _logger.LogInformation("{Count} cities retrieved successfully.", citiesDto.Count());

            return Ok(citiesDto);
        }

        // POST api/City/post/Los Angeles  -- Post the data in JSON Format
        [HttpPost("post")]
        public async Task<IActionResult> AddCity(CityDto cityDto)
        {
            _logger.LogInformation("Attempting to add a new city: {CityName}", cityDto.Name);

            var city = _mapper.Map<City>(cityDto);
            city.LastUpdatedOn = DateTime.Now;
            city.LatestUpdatedBy = 1;

            _uow.CityRepository.AddCity(city);
            await _uow.SaveAsync();

            _logger.LogInformation("City {CityName} added successfully.", cityDto.Name);

            return StatusCode(201);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCity(int id, CityDto cityDto)
        {
            _logger.LogInformation("Updating city with ID {CityId}.", id);

            try
            {
                if (id != cityDto.Id)
                {
                    _logger.LogWarning("Update not allowed for city ID mismatch: {CityId} vs {DtoId}.", id, cityDto.Id);
                    return BadRequest("Update is not allowed");
                }

                var cityFromDb = await _uow.CityRepository.FindCity(id);

                if (cityFromDb == null)
                {
                    _logger.LogWarning("City with ID {CityId} not found.", id);
                    return BadRequest("Update not allowed");
                }

                cityFromDb.LastUpdatedOn = DateTime.Now;
                cityFromDb.LatestUpdatedBy = 1;
                _mapper.Map(cityDto, cityFromDb);
                await _uow.SaveAsync();

                _logger.LogInformation("City with ID {CityId} updated successfully.", id);

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating city with ID {CityId}.", id);
                return StatusCode(500, "Some unknown error occurred.");
            }
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateCityPatch(int id, JsonPatchDocument<City> cityToPatch)
        {
            _logger.LogInformation("Patching city with ID {CityId}.", id);

            var cityFromDb = await _uow.CityRepository.FindCity(id);
            cityFromDb.LastUpdatedOn = DateTime.Now;
            cityFromDb.LatestUpdatedBy = 1;

            cityToPatch.ApplyTo(cityFromDb, ModelState);
            await _uow.SaveAsync();

            _logger.LogInformation("City with ID {CityId} patched successfully.", id);

            return StatusCode(200);
        }

        [HttpPut("updateCityName/{id}")]
        public async Task<IActionResult> UpdateCity(int id, CityUpdateDto cityDto)
        {
            _logger.LogInformation("Updating city name for city with ID {CityId}.", id);

            var cityFromDb = await _uow.CityRepository.FindCity(id);
            cityFromDb.LastUpdatedOn = DateTime.Now;
            cityFromDb.LatestUpdatedBy = 1;
            _mapper.Map(cityDto, cityFromDb);
            await _uow.SaveAsync();

            _logger.LogInformation("City name updated successfully for city with ID {CityId}.", id);

            return StatusCode(200);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            _logger.LogInformation("Deleting city with ID {CityId}.", id);

            _uow.CityRepository.DeleteCity(id);
            await _uow.SaveAsync();

            _logger.LogInformation("City with ID {CityId} deleted successfully.", id);

            return Ok(id);
        }
    }
}
