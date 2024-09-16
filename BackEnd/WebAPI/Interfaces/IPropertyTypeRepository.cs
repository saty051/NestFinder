using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IPropertyTypeRepository
    {
        Task<IEnumerable<PropertyType>> GetPropertyTypeAsync();
    }
}
