using Microsoft.EntityFrameworkCore;

using shipping_service.Persistence.Database;
using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public class PostMachineRepository : IPostMachineRepository
    {
        private readonly DatabaseContext _context;

        public PostMachineRepository(DatabaseContext ctx)
        {
            _context = ctx;
        }

        public DbSet<PostMachine> PostMachines => _context.PostMachines;

        public async Task<PostMachine?> GetAsync(long id)
        {
            return await _context.PostMachines.FindAsync(id);
        }

        public async Task CreateAsync(PostMachine postMachine)
        {
            _context.Add(postMachine);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PostMachine postMachine)
        {
            _context.Update(postMachine);
            await _context.SaveChangesAsync();
        }

        public void Delete(PostMachine postMachine)
        {
            _context.Remove(postMachine);
            _context.SaveChanges();
        }
    }
}
