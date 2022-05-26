using Microsoft.EntityFrameworkCore;

using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public interface IPostMachineRepository
    {
        DbSet<PostMachine> PostMachines { get; }

        Task<PostMachine?> GetAsync(long id);

        Task CreateAsync(PostMachine postMachine);

        Task UpdateAsync(PostMachine postMachine);

        void Delete(PostMachine postMachine);
    }
}
