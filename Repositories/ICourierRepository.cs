using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public interface ICourierRepository
    {
        IQueryable<Courier> Couriers { get; }

        Task CreateAsync(Courier courier);

        Task UpdateAsync(Courier courier);

        void DeleteAsync(Courier courier);
    }
}