using shipping_service.Persistence.Entities;
using shipping_service.Repositories;

namespace shipping_service.Services
{
    public class CourierService : ICourierService
    {
        private ICourierRepository _courierRepository;

        public CourierService(ICourierRepository repo)
        {
            _courierRepository = repo;
        }

        public async Task<Courier?> GetByIdAsync(long id)
        {
            return await _courierRepository.FindAsync(id);
        }

        public void Delete(Courier courier)
        {
            _courierRepository.Delete(courier);
        }

        public async Task<Courier?> GetByUsername(string username)
        {
            return await _courierRepository.GetByUsername(username);
        }

        public async Task UpdateAsync(Courier courier)
        {
            await _courierRepository.UpdateAsync(courier);
        }

        public async Task CreateAsync(Courier courier)
        {
            await _courierRepository.CreateAsync(courier);
        }
    }
}
