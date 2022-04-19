using Repositories.Entities;

namespace Repositories.Interfaces
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
