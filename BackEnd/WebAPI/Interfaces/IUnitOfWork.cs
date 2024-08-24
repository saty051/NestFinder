<<<<<<< HEAD
﻿using WebAPI.Repo;

namespace WebAPI.Interfaces
=======
﻿namespace WebAPI.Interfaces
>>>>>>> fffdb4469878602cf32d6fdecb601f067b9758fb
{
    public interface IUnitOfWork
    {
        ICityRepository CityRepository { get; }
<<<<<<< HEAD
=======

>>>>>>> fffdb4469878602cf32d6fdecb601f067b9758fb
        Task<bool> SaveAsync();
    }
}
