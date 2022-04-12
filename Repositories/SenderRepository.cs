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
    public class SenderRepository : ISenderRepository
    {
        private readonly DBConnectionOptions _dbConnection;
        public readonly IMongoCollection<Sender> _senders;

        public SenderRepository(IOptions<DBConnectionOptions> settings)
        {
            _dbConnection = settings.Value;
            var client = new MongoClient(_dbConnection.ConnectionString);
            var database = client.GetDatabase(_dbConnection.DatabaseName);
            _senders = database.GetCollection<Sender>("SenderCollection");
        }

        public async Task<List<Sender>> GetAsync()
        {
            var task = await _senders.FindAsync(sender => true);

            return await task.ToListAsync();
        }

        public async Task<Sender> GetAsync(string id)
        {
            var task = await _senders.FindAsync<Sender>(sender => sender._id == id);

            return await task.FirstOrDefaultAsync();
        }

        public async Task<Sender> CreateAsync(Sender sender)
        {
            await _senders.InsertOneAsync(sender);

            return sender;
        }

        public async Task<Sender> UpdateAsync(string id, Sender senderIn)
        {
            await _senders.ReplaceOneAsync(senderIn => senderIn._id == id, senderIn);

            return senderIn;
        }

        public async Task<string> DeleteAsync(string id)
        {
            await _senders.DeleteOneAsync(courier => courier._id == id);

            return id;
        }
    }
}
