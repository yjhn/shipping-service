using shipping_service.Persistence.Database;
using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public class ShipmentRepository : IShipmentRepository
    {
        private DatabaseContext context;
        public IQueryable<Shipment> Shipments => context.Shipments;

        public ShipmentRepository(DatabaseContext ctx)
        {
            context = ctx;
        }

        public async Task CreateAsync(Shipment shipment)
        {
            await context.AddAsync(shipment);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Shipment shipment)
        {
            await context.SaveChangesAsync();
        }

        public void DeleteAsync(Shipment shipment)
        {
            context.Remove(shipment);
            context.SaveChanges();
        }
    }
}