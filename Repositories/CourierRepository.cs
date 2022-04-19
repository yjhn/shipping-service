
using Microsoft.Extensions.Options;

using MongoDB.Driver;
using Repositories.Entities;
using Repositories.Interfaces;

namespace Repositories
{
    public class CourierRepository : ICourierRepository
    {
        private readonly DBConnectionOptions _dbConnection;
        public readonly IMongoCollection<Courier> _couriers;

        public CourierRepository(IOptions<DBConnectionOptions> settings)
        {
            _dbConnection = settings.Value;
            var client = new MongoClient(_dbConnection.ConnectionString);
            var database = client.GetDatabase(_dbConnection.DatabaseName);
            _couriers = database.GetCollection<Courier>("CourierCollection");
        }

        public async Task<List<Courier>> GetAsync()
        {
            var task = await _couriers.FindAsync(courier => true);

            return await task.ToListAsync();
        }

        public async Task<Courier> GetAsync(string id)
        {
            var task = await _couriers.FindAsync<Courier>(courier => courier._id == id);

            return await task.FirstOrDefaultAsync();
        }

        public async Task<Courier> CreateAsync(Courier courier)
        {
            await _couriers.InsertOneAsync(courier);

            return courier;
        }

        public async Task<Courier> UpdateAsync(string id, Courier courierIn)
        {
            await _couriers.ReplaceOneAsync(courier => courier._id == id, courierIn);

            return courierIn;
        }

        public async Task<string> DeleteAsync(string id)
        {
            await _couriers.DeleteOneAsync(courier => courier._id == id);

            return id;
        }
    }
}
