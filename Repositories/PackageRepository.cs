
using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public class PackageRepository : IPackageRepository
    {

        public async Task<List<Package>> GetAsync()
        {
            return new List<Package>();
        }

        public async Task<Package> GetAsync(ulong id)
        {
            return new Package();
        }

        public async Task<Package> CreateAsync(Package package)
        {
            return new Package();
        }

        public async Task<Package> UpdateAsync(ulong id, Package packageIn)
        {
            return new Package();
        }

        public async Task<ulong> DeleteAsync(ulong id)
        {
            return 0;
        }
    }
}
