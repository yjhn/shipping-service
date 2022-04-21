using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public interface IPackageRepository
    {
        Task<List<Package>> GetAsync();

        Task<Package> GetAsync(ulong id);

        Task<Package> CreateAsync(Package package);

        Task<Package> UpdateAsync(ulong id, Package packageIn);

        Task<ulong> DeleteAsync(ulong id);
    }
}
