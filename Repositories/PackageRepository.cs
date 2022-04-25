using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        public async Task<List<Shipment>> GetAsync()
        {
            return new List<Shipment>();
        }

        public async Task<Shipment> GetAsync(ulong id)
        {
            return new Shipment();
        }

        public async Task<Shipment> CreateAsync(Shipment shipment)
        {
            return new Shipment();
        }

        public async Task<Shipment> UpdateAsync(ulong id, Shipment shipmentIn)
        {
            return new Shipment();
        }

        public async Task<ulong> DeleteAsync(ulong id)
        {
            return 0;
        }
    }
}