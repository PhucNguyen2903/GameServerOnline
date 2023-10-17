using Database.MongoDB.Interfaces;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.MongoDB.Handlers
{
    public class MongoHandler<T> : IGameDB<T> where T : class
    {
        private IMongoDatabase _database;
        private IMongoCollection<T> _collection;

        public MongoHandler(IMongoDatabase database)
        {
            _database = database;
            this.SetCollection();
            //_collection = _database.GetCollection<T>(name: "Users");
        }

        private void SetCollection() 
        {
            switch (typeof(T).Name)
            {
                case "User":
                    //_collection = _database.GetCollection<T>("Users");
                    break;
                case "Room":
                    break;
            }
        }

        public T Create(T item)
        {
            _collection.InsertOne(item);
            return item;
        }

        public T Get(FilterDefinition<T> filter)
        {
            return _collection.Find(filter).FirstOrDefault();
        }

        public List<T> GetAll()
        {
            var filter = Builders<T>.Filter.Empty;
            return _collection.Find(filter).ToList();
        }

        public IMongoDatabase GetDatabase()
        {
            return _database;
        }

        public void Remove(FilterDefinition<T> filter)
        {
            _collection.DeleteOne(filter);
        }

        public T Update(FilterDefinition<T> filter, UpdateDefinition<T> updater)
        {
            _collection.UpdateOne(filter, updater);
            return Get(filter);
        }
    }
}
