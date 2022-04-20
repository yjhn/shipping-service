using shipping_service.Entities;

namespace shipping_service.Repositories
{
    public interface ICourierRepository
    {
        Task<List<Courier>> GetAsync();

        Task<Courier> GetAsync(string id);

        Task<Courier> CreateAsync(Courier courier);

        Task<Courier> UpdateAsync(string id, Courier courierIn);

        Task<string> DeleteAsync(string id);
    }
}
