<<<<<<< HEAD
﻿using WebAPI.Data.Repo;
using WebAPI.Interfaces;
=======
﻿using WebAPI.Interfaces;
>>>>>>> fffdb4469878602cf32d6fdecb601f067b9758fb
using WebAPI.Repo;

namespace WebAPI.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;
        public UnitOfWork(DataContext dataContext)
        {
<<<<<<< HEAD
            _dataContext = dataContext;     
        }
=======
            _dataContext = dataContext;  
        }

>>>>>>> fffdb4469878602cf32d6fdecb601f067b9758fb
        public ICityRepository CityRepository => new CityRepository(_dataContext);

        public async Task<bool> SaveAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
