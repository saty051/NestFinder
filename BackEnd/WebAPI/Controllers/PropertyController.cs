using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Mapping.ByCode.Impl;
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


        // property/list/
        [HttpGet("detail/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPropertyDetail(int id)
        {
            var property = await _uow.PropertyRepository.GetPropertyDetailAsync(id);

            var propertyDto = _mapper.Map<PropertyDetailDto>(property);

            return Ok(propertyDto);
        }

    }
}
