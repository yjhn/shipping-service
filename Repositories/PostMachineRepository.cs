using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public class PostMachineRepository : IPostMachineRepository
    {
        public async Task<List<PostMachine>> GetAsync()
        {
            return new List<PostMachine>();
        }

        public async Task<PostMachine> GetAsync(ulong id)
        {
            return new PostMachine();
        }

        public async Task<PostMachine> CreateAsync(PostMachine postMachine)
        {
            return new PostMachine();
        }

        public async Task<PostMachine> UpdateAsync(ulong id, PostMachine postMachineIn)
        {
            return new PostMachine();
        }

        public async Task<ulong> DeleteAsync(ulong id)
        {
            return 0;
        }
    }
}