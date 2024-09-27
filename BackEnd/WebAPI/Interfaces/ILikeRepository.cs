using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface ILikeRepository
    {
        Task<Like> GetLike(int userId, int propertyId);
        Task<IEnumerable<Like>> GetLikesForProperty(int propertyId);
        Task<IEnumerable<Like>> GetUserLikedProperties(int userId);
        void AddLike(Like like);
        void RemoveLike(int id);
    }
}
