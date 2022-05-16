using shipping_service.Persistence.Entities;

namespace shipping_service.Services
{
    public interface IShipmentService
    {
        IEnumerable<Shipment> GetUnassigned();
        void ChangeShipmentStatusToSrc(Shipment s, PostMachine p);
        void ChangeShipmentStatusToDest(Shipment s, PostMachine p);
        void ChangeShipmentStatusToDelivered(Shipment s);
        void ChangeShipmentStatusToShipping(Shipment s, PostMachine p);
    }
}
