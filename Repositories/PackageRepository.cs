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
    public class PackageRepository : IPackageRepository 
    {
        private readonly DBConnectionOptions _dbConnection;
        public readonly IMongoCollection<Package> _packages;

        public PackageRepository(IOptions<DBConnectionOptions> settings)
        {
            _dbConnection = settings.Value;
            var client = new MongoClient(_dbConnection.ConnectionString);
            var database = client.GetDatabase(_dbConnection.DatabaseName);
            _packages = database.GetCollection<Package>("PackageCollection");
        }

        public async Task<List<Package>> GetAsync()
        {
            var task = await _packages.FindAsync(package => true);

            return await task.ToListAsync();
        }

        public async Task<Package> GetAsync(string id)
        {
            var task = await _packages.FindAsync<Package>(package => package._id == id);

            return await task.FirstOrDefaultAsync();
        }

        public async Task<Package> CreateAsync(Package package)
        {
            await _packages.InsertOneAsync(package);

            return package;
        }

        public async Task<Package> UpdateAsync(string id, Package packageIn)
        {
            await _packages.ReplaceOneAsync(package => package._id == id, packageIn);

            return packageIn;
        }

        public async Task<string> DeleteAsync(string id)
        {
            await _packages.DeleteOneAsync(package => package._id == id);

            return id;
        }
    }
}
