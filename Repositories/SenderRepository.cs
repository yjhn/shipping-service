using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public class SenderRepository : ISenderRepository
    {
        public async Task<List<Sender>> GetAsync()
        {
            return new List<Sender>();
        }

        public async Task<Sender> GetAsync(ulong id)
        {
            return new Sender();
        }

        public async Task<Sender> CreateAsync(Sender sender)
        {
            return new Sender();
        }

        public async Task<Sender> UpdateAsync(ulong id, Sender senderIn)
        {
            return new Sender();
        }

        public async Task<ulong> DeleteAsync(ulong id)
        {
            return 0;
        }
    }
}