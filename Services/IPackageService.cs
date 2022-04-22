using shipping_service.Persistence.Entities;

namespace shipping_service.Services
{
    public interface IPackageService
    {
        Task<ICollection<Shipment>> GetUnassignedAsync();
    }
}