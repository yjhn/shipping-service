using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public interface ISenderRepository
    {
        IQueryable<Sender> Senders { get; }

        Task CreateAsync(Sender sender);

        Task UpdateAsync(Sender sender);

        void DeleteAsync(Sender sender);
    }
}
