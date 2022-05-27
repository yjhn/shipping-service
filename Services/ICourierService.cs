using shipping_service.Persistence.Entities;

namespace shipping_service.Services
{
    public interface ICourierService
    {
        Task<Courier?> GetByIdAsync(long id);
        void Delete(Courier courier);
        Task<Courier?> GetByUsername(string username);
        Task UpdateAsync(Courier courier);
        Task CreateAsync(Courier courier);
    }
}
