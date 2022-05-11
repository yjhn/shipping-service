using Microsoft.EntityFrameworkCore;

using shipping_service.Persistence.Database;
using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public class PostMachineRepository : IPostMachineRepository
    {
        private readonly DatabaseContext context;

        public PostMachineRepository(DatabaseContext ctx)
        {
            context = ctx;
        }

        public DbSet<PostMachine> PostMachines => context.PostMachines;

        public async Task<PostMachine?> GetAsync(long id)
        {
            return await context.PostMachines.FindAsync(id);
        }

        public async Task CreateAsync(PostMachine postMachine)
        {
            await context.AddAsync(postMachine);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PostMachine postMachine)
        {
            context.Update(postMachine);
            await context.SaveChangesAsync();
        }

        public void Delete(PostMachine postMachine)
        {
            context.Remove(postMachine);
            context.SaveChanges();
        }
    }
}
