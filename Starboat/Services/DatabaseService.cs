using Microsoft.Extensions.Configuration;
using Starboat.Models;
using MongoDB.Driver;

namespace Starboat.Services
{
    public class DatabaseService
    {
        private readonly MongoClient _client;
        private readonly IConfigurationRoot _config;

        public DatabaseService(IConfigurationRoot root)
        {
            _config = root;
            _client = new MongoClient(MongoUrl.Create(root["databaseUrl"]));
        }

        public IMongoDatabase Database() => _client.GetDatabase("starboat");
        public IMongoCollection<StarModel> GetCollection() => Database().GetCollection<StarModel>("stars");
    }
}