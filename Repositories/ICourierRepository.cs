using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public interface ICourierRepository
    {
        Task<List<Courier>> GetAsync();

        Task<Courier> GetAsync(ulong id);

        Task<Courier> CreateAsync(Courier courier);

        Task<Courier> UpdateAsync(ulong id, Courier courierIn);

        Task<ulong> DeleteAsync(ulong id);
    }
}