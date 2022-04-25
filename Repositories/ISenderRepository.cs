using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public interface ISenderRepository
    {
        Task<List<Sender>> GetAsync();

        Task<Sender> GetAsync(ulong id);

        Task<Sender> CreateAsync(Sender sender);

        Task<Sender> UpdateAsync(ulong id, Sender senderIn);

        Task<ulong> DeleteAsync(ulong id);
    }
}