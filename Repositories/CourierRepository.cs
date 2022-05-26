using shipping_service.Persistence.Database;
using shipping_service.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

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
            context.Entry(courier).State = EntityState.Detached;

        }

        public async Task UpdateAsync(Courier courier)
        {
            context.Update(courier);
            await context.SaveChangesAsync();
//            context.Entry(courier).State = EntityState.Detached;

        }

        public void Delete(Courier courier)
        {
            context.Remove(courier);
            context.SaveChanges();
        }

public async Task<Courier> GetByUsername(string username)
{
            foreach (var courier in Couriers)
            {
                if (courier.Username == username)
                {
                    return courier;
                }
            }
                    return null;
            }
    }
}
