using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Dtos;
using WebAPI.Interfaces;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    public class PropertyController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<PropertyController> _logger;
        private readonly IPhotoService _photoService;

        public PropertyController(IUnitOfWork uow, IMapper mapper, ILogger<PropertyController> logger, IPhotoService photoService)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _photoService = photoService;
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
        [Authorize]
        public async Task<IActionResult> AddProperty(PropertyDto propertyDto)
        {
            _logger.LogInformation("AddProperty method called with property details: {@PropertyDto}", propertyDto);

            try
            {
                // Map the DTO to the entity
                var property = _mapper.Map<Property>(propertyDto);
                var userId = GetUserId();
                property.PostedBy = userId;
                property.LatestUpdatedBy = userId;

                // Add the property to the repository
                _uow.PropertyRepository.AddProperty(property);
                await _uow.SaveAsync();

                _logger.LogInformation("Property added successfully with ID: {PropertyId}", property.Id);

                // Return a successful response with a custom message
                return StatusCode(201, new { message = "Property created successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding property: {@PropertyDto}", propertyDto);
                return StatusCode(500, new { message = "An error occurred while adding the property." });
            }
        }

        //Property/add/photo/1
        [HttpPost("add/photo/{propId}")]
        [Authorize]
        public async Task<IActionResult> AddPropertyPhoto(IFormFile file, int propId)
        {
            _logger.LogInformation("Initiating photo upload for Property ID: {PropId}", propId);

            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No file uploaded for Property ID: {PropId}", propId);
                return BadRequest("No file was uploaded.");
            }

            _logger.LogInformation("Uploading photo for Property ID: {PropId}", propId);
            var result = await _photoService.UploadPhotoAsync(file);

            if (result.Error != null)
            {
                _logger.LogError("Error uploading photo for Property ID: {PropId}. Error: {ErrorMessage}", propId, result.Error.Message);
                return BadRequest(result.Error.Message);
            }

            _logger.LogInformation("Photo uploaded successfully. Fetching property details for Property ID: {PropId}", propId);
            var property = await _uow.PropertyRepository.GetPropertyByIdAsync(propId);

            if (property == null)
            {
                _logger.LogWarning("Property with ID {PropId} not found.", propId);
                return NotFound($"Property with ID {propId} not found.");
            }

            _logger.LogInformation("Adding photo to Property ID: {PropId}", propId);
            var photo = new Photo
            {
                ImageUrl = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (property.Photos == null)
            {
                _logger.LogInformation("Initializing photo collection for Property ID: {PropId}", propId);
                property.Photos = new List<Photo>();
            }

            if (property.Photos.Count == 0)
            {
                _logger.LogInformation("Setting the uploaded photo as primary for Property ID: {PropId}", propId);
                photo.IsPrimary = true;
            }

            property.Photos.Add(photo);
            await _uow.SaveAsync();

            _logger.LogInformation("Photo added successfully to Property ID: {PropId}", propId);
            return Ok(new { message = "Photo added successfully." });
        }
    }
}
