using System.Runtime.InteropServices.ComTypes;

using shipping_service.Persistence.Database;
using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly DatabaseContext _context;

        public ShipmentRepository(DatabaseContext ctx)
        {
            _context = ctx;
        }

        public IQueryable<Shipment> Shipments => _context.Shipments;

        public async Task<Shipment?> GetAsync(long id)
        {
            return await _context.Shipments.FindAsync(id);
        }

        public async Task CreateAsync(Shipment shipment)
        {
            await _context.AddAsync(shipment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Shipment shipment)
        {
_context.Update(shipment);
            await _context.SaveChangesAsync();
        }

        public void Delete(Shipment shipment)
        {
            _context.Remove(shipment);
            _context.SaveChanges();
        }
    }
}
