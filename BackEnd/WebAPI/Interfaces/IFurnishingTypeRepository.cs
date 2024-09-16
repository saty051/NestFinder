using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IFurnishingTypeRepository
    {
        Task<IEnumerable<FurnishingType>> GetFurnishingTypeAsync();
    }
}
