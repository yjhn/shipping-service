using shipping_service.Entities;

namespace shipping_service.Repositories
{
    public class PackageRepository : IPackageRepository
    {

        public async Task<List<Package>> GetAsync()
        {
            return new List<Package>();
        }

        public async Task<Package> GetAsync(string id)
        {
            return new Package();
        }

        public async Task<Package> CreateAsync(Package package)
        {
            return new Package();
        }

        public async Task<Package> UpdateAsync(string id, Package packageIn)
        {
            return new Package();
        }

        public async Task<string> DeleteAsync(string id)
        {
            return "PlaceHolder";
        }
    }
}
