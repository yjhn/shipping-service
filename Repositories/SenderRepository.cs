using Microsoft.EntityFrameworkCore;

using shipping_service.Persistence.Database;
using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public class SenderRepository : ISenderRepository
    {
        private readonly DatabaseContext _context;

        public SenderRepository(DatabaseContext ctx)
        {
            _context = ctx;
        }

        public IQueryable<Sender> Senders => _context.Senders;

        public async Task CreateAsync(Sender sender)
        {
            // Use `Add` instead of `AddAsync`
            // https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontext.addasync?view=efcore-6.0
            _context.Add(sender);
            try
            {
                await _context.SaveChangesAsync();
            }
            finally
            {
                _context.Entry(sender).State = EntityState.Detached;
            }
        }

        public async Task UpdateAsync(Sender sender)
        {
            _context.Update(sender);
            try
            {
                await _context.SaveChangesAsync();
            }
            finally
            {
                _context.Entry(sender).State = EntityState.Detached;
            }
        }

        public void Delete(Sender sender)
        {
            _context.Remove(sender);
            _context.SaveChanges();
        }

        public async Task<Sender?> GetByUsername(string username)
        {
            return await _context.Senders.FirstOrDefaultAsync(s => s.Username == username);
        }
    }
}
