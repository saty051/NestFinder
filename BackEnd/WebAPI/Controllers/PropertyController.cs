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

                // Ensure latestUpdatedBy is included in the mapping
                var propertyDto = _mapper.Map<PropertyDetailDto>(property);
                propertyDto.LatestUpdatedBy = property.LatestUpdatedBy; // Ensure this is mapped

                _logger.LogInformation("Property details found for Property ID: {Id}", id);

                return Ok(propertyDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching property details for Property ID: {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        //Property/add
        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddProperty(PropertyDto propertyDto)
        {
            _logger.LogInformation("AddProperty method called with property details: {@PropertyDto}", propertyDto);

            try
            {
                var property = _mapper.Map<Property>(propertyDto);
                var userId = GetUserId();
                property.PostedBy = userId;
                property.LatestUpdatedBy = userId;

                _uow.PropertyRepository.AddProperty(property);
                await _uow.SaveAsync();

                _logger.LogInformation("Property added successfully with ID: {PropertyId}", property.Id);
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
        public async Task<ActionResult<PhotoDto>> AddPropertyPhoto(IFormFile file, int propId)
        {
            var userId = GetUserId();

            // Get the property by its ID
            var property = await _uow.PropertyRepository.GetPropertyByIdAsync(propId);

            if (property == null)
            {
                return NotFound($"Property with ID {propId} not found.");
            }

            // Check if the logged-in user is the one who last updated the property
            if (property.LatestUpdatedBy != userId)
            {
                return Unauthorized("You are not authorized to upload photos for this property.");
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest("No file was uploaded.");
            }

            // Proceed with uploading the photo
            var result = await _photoService.UploadPhotoAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo
            {
                ImageUrl = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (property.Photos == null)
            {
                property.Photos = new List<Photo>();
            }

            if (property.Photos.Count == 0)
            {
                photo.IsPrimary = true;
            }

            property.Photos.Add(photo);

            if (await _uow.SaveAsync())
            {
                return _mapper.Map<PhotoDto>(photo);
            }

            return BadRequest("Some problem occurred in uploading the photo, please retry.");
        }


        //Property/set-primary-photo/1/sfdghghfh
        [HttpPost("set-primary-photo/{propId}/{photoPublicId}")]
        [Authorize]
        public async Task<IActionResult> SetPrimaryPhoto(int propId, string photoPublicId)
        {
            var userId = GetUserId();

            var property = await _uow.PropertyRepository.GetPropertyByIdAsync(propId);

            if (property.PostedBy != userId)
                return BadRequest("You are not authorised to change the photo");

            if (property == null || property.PostedBy != userId)
                return BadRequest("No such property or photo exists");

            var photo = property.Photos.FirstOrDefault(p => p.PublicId == photoPublicId);

            if (photo == null)
                return BadRequest("No such property or photo exists");

            if (photo.IsPrimary)
                return BadRequest("This is already a primary photo");


            var currentPrimary = property.Photos.FirstOrDefault(p => p.IsPrimary);
            if (currentPrimary != null) currentPrimary.IsPrimary = false;
            photo.IsPrimary = true;

            if (await _uow.SaveAsync()) return NoContent();

            return BadRequest("Failed to set primary photo");
        }

        //Property/delete-photo/1/sfdghghfh
        [HttpDelete("delete-photo/{propId}/{photoPublicId}")]
        [Authorize]
        public async Task<IActionResult> DeletePhoto(int propId, string photoPublicId)
        {
            var userId = GetUserId();

            var property = await _uow.PropertyRepository.GetPropertyByIdAsync(propId);

            if (property.PostedBy != userId)
                return BadRequest("You are not authorised to delete the photo");

            if (property == null || property.PostedBy != userId)
                return BadRequest("No such property or photo exists");

            var photo = property.Photos.FirstOrDefault(p => p.PublicId == photoPublicId);

            if (photo == null)
                return BadRequest("No such property or photo exists");

            if (photo.IsPrimary)
                return BadRequest("You can not delete primary photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            property.Photos.Remove(photo);

            if (await _uow.SaveAsync()) return Ok();

            return BadRequest("Failed to delete photo");
        }
    }
}
