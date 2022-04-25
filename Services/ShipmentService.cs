using shipping_service.Persistence.Entities;
using shipping_service.Repositories;

namespace shipping_service.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _shipmentRepository;

        public ShipmentService(IShipmentRepository shipmentRepository)
        {
            _shipmentRepository = shipmentRepository;
        }

        public IEnumerable<Shipment> GetUnassigned()
        {
            return _shipmentRepository.Shipments.Where(s => s.CourierId == null);
        }
    }
}
