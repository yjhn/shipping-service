using shipping_service.Persistence.Entities;
using shipping_service.Repositories;
using Microsoft.EntityFrameworkCore;
namespace shipping_service.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _shipmentRepository;

        public ShipmentService(IShipmentRepository shipmentRepository)
        {
            _shipmentRepository = shipmentRepository;
        }

        public async Task<IEnumerable<Shipment>> GetUnassignedAsync()
        {
            return await _shipmentRepository.Shipments.
                Where(s => s.CourierId == null).
            Include(s => s.Sender).
            Include(s => s.Courier).
            Include(s => s.SourceMachine).
            Include(s => s.DestinationMachine).ToListAsync();
        }

        public async Task<IEnumerable<Shipment>> GetAssignedAsync(long courierId)
        {
            return await _shipmentRepository.Shipments.
    Where(s => s.CourierId == courierId).
    Include(s => s.Sender).
    Include(s => s.Courier).
    Include(s => s.SourceMachine).
    Include(s => s.DestinationMachine).ToListAsync();
        }
    }
}
