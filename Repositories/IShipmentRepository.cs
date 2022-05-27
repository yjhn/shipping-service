using shipping_service.Models;
using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public interface IShipmentRepository
    {
        IQueryable<Shipment> Shipments { get; }
        Task<Shipment?> GetAsync(long id);
        Task<Shipment?> GetBypassCache(long id);

        Task CreateAsync(Shipment shipment);

        Task<DbUpdateResult> UpdateAsync(Shipment shipment);
        void Delete(Shipment shipment);
        void Detach(long id);
    }
}
