using Microsoft.Extensions.Options;

using shipping_service.Entities;

namespace shipping_service.Repositories
{
    public class PostMachineRepository : IPostMachineRepository
    {

        public async Task<List<PostMachine>> GetAsync()
        {
            return new List<PostMachine>();
        }

        public async Task<PostMachine> GetAsync(string id)
        {
            return new PostMachine();
        }

        public async Task<PostMachine> CreateAsync(PostMachine postMachine)
        {
            return new PostMachine();
        }

        public async Task<PostMachine> UpdateAsync(string id, PostMachine postMachineIn)
        {
            return new PostMachine();
        }

        public async Task<string> DeleteAsync(string id)
        {
            return "PlaceHolder";
        }

    }
}
