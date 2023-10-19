using Database.MongoDB.Handlers;
using Database.MongoDB.Interfaces;
using GameServer_Demo.GameModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Room
{
    public class RoomHandler : IDbHandler<RoomModel>
    {
        private readonly IGameDB<RoomModel> _roomDb;

        public RoomHandler(IMongoDatabase roomDb)
        {
            _roomDb = new MongoHandler<RoomModel>(roomDb);
        }

        public RoomModel Create(RoomModel item)
        {
            throw new NotImplementedException();
        }

        public RoomModel Find(string id)
        {
            throw new NotImplementedException();
        }

        public List<RoomModel> FindAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }

        public RoomModel Update(string id, RoomModel item)
        {
            throw new NotImplementedException();
        }
    }
}
