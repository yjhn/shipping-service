using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public interface IShipmentRepository
    {
        IQueryable<Shipment> Shipments { get; }
        Task<Shipment?> GetAsync(long id);

        Task CreateAsync(Shipment shipment);

        Task UpdateAsync(Shipment shipment);

        void Delete(Shipment shipment);
    }
}
