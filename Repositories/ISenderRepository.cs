using shipping_service.Entities;

namespace shipping_service.Repositories
{
    public interface ISenderRepository
    {
        Task<List<Sender>> GetAsync();

        Task<Sender> GetAsync(string id);

        Task<Sender> CreateAsync(Sender sender);

        Task<Sender> UpdateAsync(string id, Sender senderIn);

        Task<string> DeleteAsync(string id);
    }
}
