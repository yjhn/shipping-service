using shipping_service.Persistence.Database;
using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public class CourierRepository : ICourierRepository
    {
        private readonly DatabaseContext context;

        public CourierRepository(DatabaseContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Courier> Couriers => context.Couriers;

        public async Task CreateAsync(Courier courier)
        {
            await context.AddAsync(courier);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Courier courier)
        {
            await context.SaveChangesAsync();
        }

        public void Delete(Courier courier)
        {
            context.Remove(courier);
            context.SaveChanges();
        }
    }
}
