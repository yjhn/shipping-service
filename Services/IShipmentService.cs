using shipping_service.Persistence.Entities;

namespace shipping_service.Services
{
    public interface IShipmentService
    {
        IEnumerable<Shipment> GetUnassigned();
    }
}
