using shipping_service.Persistence.Entities;

namespace shipping_service.Repositories
{
    public class CourierRepository : ICourierRepository
    {
        public async Task<List<Courier>> GetAsync()
        {
            return new List<Courier>();
        }

        public async Task<Courier> GetAsync(ulong id)
        {
            return new Courier();
        }

        public async Task<Courier> CreateAsync(Courier courier)
        {
            return new Courier();
        }

        public async Task<Courier> UpdateAsync(ulong id, Courier courierIn)
        {
            return new Courier();
        }

        public async Task<ulong> DeleteAsync(ulong id)
        {
            return 0;
        }
    }
}