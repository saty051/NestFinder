using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers
    {
    [Authorize]
    public class LikeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<LikeController> _logger;

        public LikeController(IUnitOfWork uow, IMapper mapper, ILogger<LikeController> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Like/property/{propertyId}
        [AllowAnonymous]
        [HttpGet("property/{propertyId}")]
        public async Task<IActionResult> GetLikesForProperty(int propertyId)
        {
            _logger.LogInformation("Fetching likes for property with ID {PropertyId}.", propertyId);

            var likes = await _uow.LikeRepository.GetLikesForProperty(propertyId);

            if (!likes.Any())
            {
                _logger.LogInformation("No likes found for property with ID {PropertyId}.", propertyId);
                return NotFound("No likes found.");
            }

            var likeDtos = _mapper.Map<IEnumerable<LikeDto>>(likes);

            _logger.LogInformation("{Count} likes found for property with ID {PropertyId}.", likeDtos.Count(), propertyId);

            return Ok(likeDtos);
        }

        // POST: api/Like/{propertyId}
        [HttpPost("{propertyId}")]
        public async Task<IActionResult> AddLike(int propertyId)
        {
            var userId = GetUserId();
            _logger.LogInformation("Attempting to add a like by user ID {UserId} for property ID {PropertyId}.", userId, propertyId);

            var like = await _uow.LikeRepository.GetLike(userId, propertyId);
            if (like != null)
            {
                _logger.LogWarning("User ID {UserId} has already liked property ID {PropertyId}.", userId, propertyId);
                return BadRequest("You already liked this property.");
            }

            var newLike = new Like
            {
                UserId = userId,
                PropertyId = propertyId,
                LikedOn = DateTime.Now
            };

            _uow.LikeRepository.AddLike(newLike);
            if (await _uow.SaveAsync())
            {
                _logger.LogInformation("User ID {UserId} successfully liked property ID {PropertyId}.", userId, propertyId);
                return Ok("Property liked.");
            }

            _logger.LogError("Failed to save like by user ID {UserId} for property ID {PropertyId}.", userId, propertyId);
            return BadRequest("Failed to like property.");
        }

        // DELETE: api/Like/{likeId}
        [HttpDelete("{propertyId}")]
        public async Task<IActionResult> RemoveLike(int propertyId)
        {
            // Get the logged-in user's ID from the token or session
            var userId = GetUserId(); // Ensure you have a method to get the logged-in user ID

            // Get the like based on the user and property
            var like = await _uow.LikeRepository.GetLike(userId, propertyId);

            if (like == null)
            {
                return NotFound("Like not found");
            }

            _uow.LikeRepository.RemoveLike(like.Id);
            await _uow.SaveAsync();

            return Ok("Like removed successfully");
        }



        // GET: api/Like/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserLikedProperties(int userId)
        {
            _logger.LogInformation("Fetching liked properties for user ID {UserId}.", userId);

            var likes = await _uow.LikeRepository.GetUserLikedProperties(userId);
            if (!likes.Any())
            {
                _logger.LogInformation("No properties liked by user ID {UserId}.", userId);
                return NotFound("No liked properties found.");
            }

            var likeDtos = _mapper.Map<IEnumerable<LikeDto>>(likes);

            _logger.LogInformation("{Count} liked properties found for user ID {UserId}.", likeDtos.Count(), userId);

            return Ok(likeDtos);
        }
    }
}
