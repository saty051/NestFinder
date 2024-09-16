using Microsoft.EntityFrameworkCore;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Data.Repo
{
    public class FurnishingTypeRepository : IFurnishingTypeRepository
    {
        private readonly DataContext _dataContext;
        public FurnishingTypeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IEnumerable<FurnishingType>> GetFurnishingTypeAsync()
        {
            return await _dataContext.FurnishingTypes.ToListAsync();
        }
    }
}
