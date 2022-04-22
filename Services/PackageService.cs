using shipping_service.Persistence.Entities;
using shipping_service.Repositories;

namespace shipping_service.Services
{
    public class PackageService : IPackageService
    {
        private readonly ICourierRepository _courierRepository;
        private readonly IPackageRepository _packageRepository;

        public PackageService(IPackageRepository packageRepository, ICourierRepository courierRepository)
        {
            _packageRepository = packageRepository;
            _courierRepository = courierRepository;
        }

        public async Task<ICollection<Package>> GetUnassignedAsync()
        {
            List<Package>? packages = await _packageRepository.GetAsync();
            List<Courier> couriers = await _courierRepository.GetAsync();
            foreach (Courier courier in couriers)
            {
                ICollection<Package> courierPackages = courier.CurrentPackages;
                if (packages != null)
                {
                    foreach (Package package in courierPackages)
                    {
                        packages.Remove(package);
                    }
                }
            }

            return packages;
        }
    }
}