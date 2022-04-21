using shipping_service.Entities;

namespace shipping_service.Repositories
{
    public class CourierRepository : ICourierRepository
    {

        public CourierRepository()
        {
        }

        public async Task<List<Courier>> GetAsync()
        {
            return new List<Courier>();
        }

        public async Task<Courier> GetAsync(string id)
        {
            return new Courier();
        }

        public async Task<Courier> CreateAsync(Courier courier)
        {
            return new Courier();
        }

        public async Task<Courier> UpdateAsync(string id, Courier courierIn)
        {
            return new Courier();
        }

        public async Task<string> DeleteAsync(string id)
        {
            return "PlaceHolder";
        }
    }
}
