
using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public interface IPostMachineRepository
    {
        Task<List<PostMachine>> GetAsync();

        Task<PostMachine> GetAsync(ulong id);

        Task<PostMachine> CreateAsync(PostMachine postMachine);

        Task<PostMachine> UpdateAsync(ulong id, PostMachine postMachineIn);

        Task<ulong> DeleteAsync(ulong id);

    }
}
