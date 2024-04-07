using GameServer_Demo.Application.Messaging.Contains;
using GameServer_Demo.Room.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Room.Interfaces
{
    public interface IRoomManager
    {
        Lobby Lobby { get; set; }
        BaseRoom CreateRoom(int timer, string ownerId = "");
        BaseRoom FindRoom(string id);

        List<BaseRoom> ListRoom();
        bool RemoveRoom(string Id);
    }
}
