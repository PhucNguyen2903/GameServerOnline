using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.MongoDB.Interfaces
{
    public interface IGameDB<T> where T : class
    {
        IMongoDatabase GetDatabase();
        T Get(string id);
        List<T> GetAll();
        T Create(T item);
        bool Remove(T item);
        T Update(string id, T item);
    }
}
