using shipping_service.Entities;

namespace shipping_service.Services
{
    public interface IPackageService
    {
        Task<List<Package>> GetUnassignedAsync();
    }
}
