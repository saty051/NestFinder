
using WebAPI.Data.Repo;
using WebAPI.Interfaces;
using WebAPI.Repositories;



namespace WebAPI.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;

        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;  
        }
        public ICityRepository CityRepository => new CityRepository(_dataContext);

        public IUserRepository UserRepository =>  new UserRepository(_dataContext);

        public IPropertyRepository PropertyRepository => new PropertyRepository(_dataContext);

        public IPropertyTypeRepository PropertyTypeRepository =>  new PropertyTypeRepository(_dataContext);

        public IFurnishingTypeRepository FurnishingTypeRepository => new FurnishingTypeRepository(_dataContext);

        public ILikeRepository LikeRepository => new LikeRepository(_dataContext);

        public async Task<bool> SaveAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
