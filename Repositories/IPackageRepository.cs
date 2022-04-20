using shipping_service.Entities;

namespace shipping_service.Repositories
{
    public interface IPackageRepository
    {
        Task<List<Package>> GetAsync();

        Task<Package> GetAsync(string id);

        Task<Package> CreateAsync(Package package);

        Task<Package> UpdateAsync(string id, Package packageIn);

        Task<string> DeleteAsync(string id);
    }
}
