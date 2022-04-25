using shipping_service.Persistence.Database;
using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public class SenderRepository : ISenderRepository
    {
        private readonly DatabaseContext context;

        public SenderRepository(DatabaseContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Sender> Senders => context.Senders;

        public async Task CreateAsync(Sender sender)
        {
            await context.AddAsync(sender);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Sender sender)
        {
            await context.SaveChangesAsync();
        }

        public void DeleteAsync(Sender sender)
        {
            context.Remove(sender);
            context.SaveChanges();
        }
    }
}
