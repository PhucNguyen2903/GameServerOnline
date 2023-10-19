using GameServer_Demo.Room.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Room.Handlers
{
    public class RoomManager : IRoomManager
    {
        public BaseRoom Lobby { get; set; }
        private ConcurrentDictionary<string, BaseRoom> Rooms { get; set; }

        public RoomManager()
        {
            Rooms = new ConcurrentDictionary<string, BaseRoom>();
            Lobby = new BaseRoom();
        }
        public BaseRoom FindRoom(string id)
        {
            return Rooms.FirstOrDefault(r => r.Key.Equals(id)).Value;
        }

        public bool RemoveRoom(string Id)
        {
            var oldRoom = FindRoom(Id);
            if (oldRoom != null)
            {
                Rooms.TryRemove(Id, out var room);
                return room != null;
            }
            return false;
        }

        public BaseRoom CreateRoom()
        {
            var room = new BaseRoom();
            Rooms.TryAdd(room.Id, room);
            return room;
        }
    }
}
