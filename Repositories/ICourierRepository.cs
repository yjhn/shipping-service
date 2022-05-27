using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public interface ICourierRepository
    {
        IQueryable<Courier> Couriers { get; }

        Task<Courier?> FindAsync(long id);

        Task CreateAsync(Courier courier);

        Task UpdateAsync(Courier courier);

        void Delete(Courier courier);
        Task<Courier?> GetByUsername(string username);
    }
}
