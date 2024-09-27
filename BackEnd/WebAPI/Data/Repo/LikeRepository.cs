using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly DataContext _context;

        public LikeRepository(DataContext context)
        {
            _context = context;
        }

        // Get all likes for a specific property
        public async Task<IEnumerable<Like>> GetLikesForProperty(int propertyId)
        {
            return await _context.Likes
                .Where(like => like.PropertyId == propertyId)
                .Include(l => l.User)
                .Include(l => l.Property)
                .ToListAsync();
        }

        // Add a new like
        public void AddLike(Like like)
        {
            _context.Likes.Add(like);
        }

        // Delete a like by its id
        public void RemoveLike(int likeId)
        {
            var like = _context.Likes.Find(likeId);
            if (like != null)
            {
                _context.Likes.Remove(like);
            }
        }

        // Find a like by its userId and propertyId
        public async Task<Like> GetLike(int userId, int propertyId)
        {
            return await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.PropertyId == propertyId);
        }

        // Get all properties liked by a user
        public async Task<IEnumerable<Like>> GetUserLikedProperties(int userId)
        {
            return await _context.Likes
                .Where(like => like.UserId == userId)
                .Include(l => l.User)
                .Include(l => l.Property)
                .ToListAsync();
        }
    }
}
