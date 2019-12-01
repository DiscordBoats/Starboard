using Microsoft.Extensions.Configuration;
using Starboat.Models;
using MongoDB.Driver;

namespace Starboat.Services
{
    public class DatabaseService
    {
        public MongoClient client;
        public IMongoDatabase database;
        public IMongoCollection<StarModel> stars;
        private readonly IConfigurationRoot config;

        public DatabaseService(IConfigurationRoot root)
        {
            config = root;
            client = new MongoClient();
        }
    }
}