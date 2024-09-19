using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Dtos;
using WebAPI.Interfaces;

namespace WebAPI.Controllers
{
    public class PropertyController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<PropertyController> _logger;

        public PropertyController(IUnitOfWork uow, IMapper mapper, ILogger<PropertyController> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        // property/list/
        [HttpGet("list/{sellRent}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPropertyList(int sellRent)
        {
            _logger.LogInformation("Fetching property list for SellRent value: {SellRent}", sellRent);

            try
            {
                var properties = await _uow.PropertyRepository.GetPropertiesAsync(sellRent);
                if (properties == null || !properties.Any())
                {
                    _logger.LogWarning("No properties found for SellRent value: {SellRent}", sellRent);
                    return NotFound("No properties available.");
                }

                var propertyListDto = _mapper.Map<IEnumerable<PropertyListDto>>(properties);
                _logger.LogInformation("{Count} properties found for SellRent value: {SellRent}", propertyListDto.Count(), sellRent);

                return Ok(propertyListDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching property list for SellRent value: {SellRent}", sellRent);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        // property/detail/
        [HttpGet("detail/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPropertyDetail(int id)
        {
            _logger.LogInformation("Fetching property details for Property ID: {Id}", id);

            try
            {
                var property = await _uow.PropertyRepository.GetPropertyDetailAsync(id);

                if (property == null)
                {
                    _logger.LogWarning("No details found for Property ID: {Id}", id);
                    return NotFound("No property details available.");
                }

                var propertyDto = _mapper.Map<PropertyDetailDto>(property);
                _logger.LogInformation("Property details found for Property ID: {Id}", id);

                return Ok(propertyDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching property details for Property ID: {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("add")]
        [AllowAnonymous]
        public async Task<IActionResult> AddProperty(PropertyDto propertyDto)
        {
            _logger.LogInformation("Attempting to add a new property with details: {@PropertyDto}", propertyDto);

            if (propertyDto == null || !ModelState.IsValid)
            {
                _logger.LogWarning("Invalid data for property: {@PropertyDto}", propertyDto);
                return BadRequest("Invalid data provided.");
            }

            try
            {
                var property = _mapper.Map<Property>(propertyDto);
                property.PostedBy = 1; // Replace with actual user ID in production
                property.LatestUpdatedBy = 1;

                _uow.PropertyRepository.AddProperty(property);
                await _uow.SaveAsync();

                _logger.LogInformation("Property successfully added with ID: {PropertyId}", property.Id);
                return StatusCode(201, "Property created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding property with details: {@PropertyDto}", propertyDto);
                return StatusCode(500, "An error occurred while adding the property.");
            }
        }


    }
}
