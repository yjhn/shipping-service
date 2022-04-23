using shipping_service.Persistence.Entities;
using shipping_service.Repositories;

namespace shipping_service.Services
{
    public class PackageService : IPackageService
    {
        private readonly ICourierRepository _courierRepository;
        private readonly IShipmentRepository _packageRepository;

        public PackageService(IShipmentRepository packageRepository, ICourierRepository courierRepository)
        {
            _packageRepository = packageRepository;
            _courierRepository = courierRepository;
        }

        public IEnumerable<Shipment> GetUnassignedAsync()
        {
            return _packageRepository.Shipments.Where(s => s.CourierId == null);
        }
    }
}