using Repositories.Entities;
using Repositories.Interfaces;

namespace Services
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _packageRepository;
        private readonly ICourierRepository _courierRepository;

        public PackageService(IPackageRepository packageRepository, ICourierRepository courierRepository)
        {
            _packageRepository = packageRepository;
            _courierRepository = courierRepository;
        }

        public async Task<List<Package>> GetUnassignedAsync()
        {
            var packages = await _packageRepository.GetAsync();
            var couriers = await _courierRepository.GetAsync();
            foreach (Courier courier in couriers)
            {
                var courierPackages = courier.Packages;
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