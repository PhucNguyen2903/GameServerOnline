using Database.MongoDB.Handlers;
using Database.MongoDB.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.GameModel.Handlers
{
    public class UserHandlers : IDbHandler<User>
    {
        private readonly IGameDB<User> _userDb;

        public UserHandlers(IMongoDatabase database) 
        {
            _userDb = new MongoHandler<User>(database);
        }

        public User Create(User item)
        {
            var user = _userDb.Create(item);
            return user;
        }

        public User Find(string id)
        {
            var filter = Builders<User>.Filter.Eq(i => i.Id,id);
            return _userDb.Get(filter);
        }

        public User FindByUserName(string username)
        {
            var filter = Builders<User>.Filter.Eq(i => i.UserName, username);
            return _userDb.Get(filter);
        }

        public List<User> FindAll()
        {
           return _userDb.GetAll();
        }

        public void Remove(string id)
        {
            var filter = Builders<User>.Filter.Eq(i => i.Id,id);
            _userDb.Remove(filter);
        }

        public User Update(string id, User item)
        {
            var filter = Builders<User>.Filter.Eq(i => i.Id, id);
            var updater = Builders<User>.Update.Set(i => i.Password, item.Password).
                Set(i => i.DisplayName, item.DisplayName).Set(i => i.Amount, item.Amount).
                Set(i => i.Level, item.Level).Set(i => i.Avatar, item.Avatar).Set(i => i.UpdatedAt, DateTime.Now);

            _userDb.Update(filter, updater);
            return item;
        }
    }
}
