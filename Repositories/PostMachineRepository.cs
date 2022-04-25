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

        public IQueryable<PostMachine> PostMachines => context.PostMachines;

        public async Task CreateAsync(PostMachine postMachine)
        {
            await context.AddAsync(postMachine);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PostMachine postMachine)
        {
            await context.SaveChangesAsync();
        }

        public void Delete(PostMachine postMachine)
        {
            context.Remove(postMachine);
            context.SaveChanges();
        }
    }
}
