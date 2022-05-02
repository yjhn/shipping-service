using shipping_service.Persistence.Database;
using shipping_service.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                await context.SaveChangesAsync();
            }
            finally
            {
                context.Entry(sender).State = EntityState.Detached;
            }
        }

        public async Task UpdateAsync(Sender sender)
        {
            context.Update(sender);
try
{
            await context.SaveChangesAsync();
}
finally
{
            context.Entry(sender).State = EntityState.Detached;
        }
}
        public void Delete(Sender sender)
        {
            context.Remove(sender);
            context.SaveChanges();
        }

        public async Task<Sender> GetByUsername(string username)
        {
            foreach (var sender in Senders)
            {
                if (sender.Username == username)
                {
                    return sender;
                }
            }
            return null;
            }

        }
    }
