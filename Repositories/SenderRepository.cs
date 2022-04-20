using Microsoft.Extensions.Options;

using shipping_service.Entities;

namespace shipping_service.Repositories
{
    public class SenderRepository : ISenderRepository
    {
        public async Task<List<Sender>> GetAsync()
        {
            return new List<Sender>();
        }

        public async Task<Sender> GetAsync(string id)
        {
            return new Sender();
        }

        public async Task<Sender> CreateAsync(Sender sender)
        {
            return new Sender();
        }

        public async Task<Sender> UpdateAsync(string id, Sender senderIn)
        {
            return new Sender();
        }

        public async Task<string> DeleteAsync(string id)
        {
            return "PlaceHolder";
        }
    }
}
