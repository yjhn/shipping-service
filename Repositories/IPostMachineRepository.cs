using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public interface IPostMachineRepository
    {
        IQueryable<PostMachine> PostMachines { get; }

        Task CreateAsync(PostMachine postMachine);

        Task UpdateAsync(PostMachine postMachine);

        void Delete(PostMachine postMachine);
    }
}
