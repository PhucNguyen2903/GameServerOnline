using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.MongoDB.Interfaces;
using MongoDB.Driver;

namespace Database.MongoDB.Handlers
{
    public class MongoDb
    {
        private readonly IMongoClient _client;
        string path = "mongodb://localhost:27017/";
        private IMongoDatabase _database =>  _client.GetDatabase(name: "GameOnline");

        public MongoDb()
        {
            var setting = MongoClientSettings.FromConnectionString(path);
            _client = new MongoClient(setting);

        }

        public IMongoDatabase GetDatabase()
        {
            return _database;
        }
    }
}
