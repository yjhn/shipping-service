using shipping_service.Persistence.Entities;
using shipping_service.Repositories;

namespace shipping_service.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _packageRepository;

        public ShipmentService(IShipmentRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        public IEnumerable<Shipment> GetUnassigned()
        {
            return _packageRepository.Shipments.Where(s => s.CourierId == null);
        }
    }
}
