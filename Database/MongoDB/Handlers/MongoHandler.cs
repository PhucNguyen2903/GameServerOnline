using Database.MongoDB.Interfaces;
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
            //_collection = _database.GetCollection<T>(name:"Users");
        }

        public T Create(T item)
        {
           _collection.InsertOne(item);
            return item;
        }

        public T Get(string id)
        {
            //return _collection.Find<User>(it => it.Id == id).FirstOrDefault();
            return default(T);
        }

        public List<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public IMongoDatabase GetDatabase()
        {
            return _database;
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public T Update(string id, T item)
        {
            throw new NotImplementedException();
        }
    }
}
