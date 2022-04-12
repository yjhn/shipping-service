using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using MongoDB.Driver;

using Repositories.Entities;
using Repositories.Interfaces;

namespace Repositories
{
    public class PostMachineRepository : IPostMachineRepository
    {
        private readonly DBConnectionOptions _dbConnection;
        public readonly IMongoCollection<PostMachine> _postMachines;

        public PostMachineRepository(IOptions<DBConnectionOptions> settings)
        {
            _dbConnection = settings.Value;
            var client = new MongoClient(_dbConnection.ConnectionString);
            var database = client.GetDatabase(_dbConnection.DatabaseName);
            _postMachines = database.GetCollection<PostMachine>("PostMachineCollection");
        }

        public async Task<List<PostMachine>> GetAsync()
        {
            var task = await _postMachines.FindAsync(postMachine => true);

            return await task.ToListAsync();
        }

        public async Task<PostMachine> GetAsync(string id)
        {
            var task = await _postMachines.FindAsync<PostMachine>(postMachine => postMachine._id == id);

            return await task.FirstOrDefaultAsync();
        }

        public async Task<PostMachine> CreateAsync(PostMachine postMachine)
        {
            await _postMachines.InsertOneAsync(postMachine);

            return postMachine;
        }

        public async Task<PostMachine> UpdateAsync(string id, PostMachine postMachineIn)
        {
            await _postMachines.ReplaceOneAsync(postMachine => postMachine._id == id, postMachineIn);

            return postMachineIn;
        }

        public async Task<string> DeleteAsync(string id)
        {
            await _postMachines.DeleteOneAsync(postMachine => postMachine._id == id);

            return id;
        }

    }
}
