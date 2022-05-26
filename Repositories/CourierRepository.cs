using Microsoft.EntityFrameworkCore;

using shipping_service.Persistence.Database;
using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public class CourierRepository : ICourierRepository
    {
        private readonly DatabaseContext _context;

        public CourierRepository(DatabaseContext ctx)
        {
            _context = ctx;
        }

        public IQueryable<Courier> Couriers => _context.Couriers;

        public async Task CreateAsync(Courier courier)
        {
            _context.Add(courier);
            await _context.SaveChangesAsync();
            _context.Entry(courier).State = EntityState.Detached;
        }

        public async Task UpdateAsync(Courier courier)
        {
            _context.Update(courier);
            await _context.SaveChangesAsync();
        }

        public void Delete(Courier courier)
        {
            _context.Remove(courier);
            _context.SaveChanges();
        }

        public async Task<Courier?> GetByUsername(string username)
        {
            return await _context.Couriers.FirstOrDefaultAsync(c => c.Username == username);
        }
    }
}
