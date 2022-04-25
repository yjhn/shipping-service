using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public interface IPackageRepository
    {
        Task<List<Shipment>> GetAsync();

        Task<Shipment> GetAsync(ulong id);

        Task<Shipment> CreateAsync(Shipment shipment);

        Task<Shipment> UpdateAsync(ulong id, Shipment shipmentIn);

        Task<ulong> DeleteAsync(ulong id);
    }
}