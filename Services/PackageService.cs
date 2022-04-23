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

        public async Task<ICollection<Shipment>> GetUnassignedAsync()
        {
            List<Shipment>? packages = await _packageRepository.GetAsync();
            List<Courier> couriers = await _courierRepository.GetAsync();
            foreach (Courier courier in couriers)
            {
                ICollection<Shipment> courierPackages = courier.CurrentPackages;
                if (packages != null)
                {
                    foreach (Shipment package in courierPackages)
                    {
                        packages.Remove(package);
                    }
                }
            }

            return packages;
        }
    }
}