using Microsoft.EntityFrameworkCore;

using shipping_service.Models;
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

        public async Task<Shipment?> GetBypassCache(long id)
        {
            return await _context.Shipments.AsNoTracking().SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task CreateAsync(Shipment shipment)
        {
            _context.Add(shipment);
            await _context.SaveChangesAsync();
        }

        public async Task<DbUpdateResult> UpdateAsync(Shipment shipment)
        {
            Shipment s = (await _context.Shipments.FindAsync(shipment.Id))!;
            // Existing entity has to be detached, because in some cases `shipment` is detached.
            _context.Entry(s).State = EntityState.Detached;
            _context.Update(shipment);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return DbUpdateResult.ConcurrentUpdateError;
            }

            return DbUpdateResult.Success;
        }

        public void Delete(Shipment shipment)
        {
            _context.Remove(shipment);
            _context.SaveChanges();
        }

        // Should only be called when it is known that the entity is cached
        // by the DbContext.
        public void Detach(long id)
        {
            Shipment s = _context.Shipments.Find(id)!;
            _context.Entry(s).State = EntityState.Detached;
        }
    }
}
