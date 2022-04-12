﻿using Repositories.Entities;

namespace Repositories.Interfaces
{
    public interface ICourierRepository
    {
        Task<List<Courier>> GetAsync();

        Task<Courier> GetAsync(string id);

        Task<Courier> CreateAsync(Courier courier);

        Task<Courier> UpdateAsync(string id, Courier courierIn);

        Task<string> DeleteAsync(string id);
    }
}
