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
        // Anonymous users can view the total likes for a property
        [AllowAnonymous]
        [HttpGet("property/{propertyId}")]
        public async Task<IActionResult> GetLikesForProperty(int propertyId)
        {
            _logger.LogInformation("Fetching total likes for property with ID {PropertyId}.", propertyId);

            var likes = await _uow.LikeRepository.GetLikesForProperty(propertyId);
            var totalLikes = likes.Count();

            _logger.LogInformation("Total likes for property ID {PropertyId}: {TotalLikes}.", propertyId, totalLikes);

            return Ok(totalLikes);
        }

        // GET: api/Like/check/{propertyId}
        // Only logged-in users can check if they've liked a property
        [HttpGet("check/{propertyId}")]
        public async Task<IActionResult> IsPropertyLiked(int propertyId)
        {
            var userId = GetUserId();
            if (userId == 0)
            {
                // Return 401 Unauthorized if the user is not logged in
                return Unauthorized("You need to be logged in to check if you've liked a property.");
            }

            var like = await _uow.LikeRepository.GetLike(userId, propertyId);
            return Ok(like != null);
        }

        // POST: api/Like/{propertyId}
        // Requires authorization to like a property
        [HttpPost("{propertyId}")]
        public async Task<IActionResult> AddLike(int propertyId)
        {
            var userId = GetUserId();
            if (userId == 0)
            {
                return Unauthorized("You need to be logged in to like a property.");
            }

            _logger.LogInformation("User ID {UserId} attempting to like property ID {PropertyId}.", userId, propertyId);

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

        // DELETE: api/Like/{propertyId}
        // Requires authorization to remove a like
        [HttpDelete("{propertyId}")]
        public async Task<IActionResult> RemoveLike(int propertyId)
        {
            var userId = GetUserId();
            if (userId == 0)
            {
                return Unauthorized("You need to be logged in to remove a like.");
            }

            var like = await _uow.LikeRepository.GetLike(userId, propertyId);
            if (like == null)
            {
                _logger.LogWarning("No like found for user ID {UserId} on property ID {PropertyId}.", userId, propertyId);
                return NotFound("Like not found.");
            }

            _uow.LikeRepository.RemoveLike(like.Id);
            if (await _uow.SaveAsync())
            {
                _logger.LogInformation("User ID {UserId} removed like for property ID {PropertyId}.", userId, propertyId);
                return Ok("Like removed successfully.");
            }

            _logger.LogError("Failed to remove like by user ID {UserId} for property ID {PropertyId}.", userId, propertyId);
            return BadRequest("Failed to remove like.");
        }
    }
}
