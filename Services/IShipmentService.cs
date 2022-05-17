using shipping_service.Persistence.Entities;

namespace shipping_service.Services
{
    public interface IShipmentService
    {
Task<IEnumerable<Shipment>> GetUnassignedAsync();
        Task<IEnumerable<Shipment>> GetAssignedAsync(long courierId);
    }
}
