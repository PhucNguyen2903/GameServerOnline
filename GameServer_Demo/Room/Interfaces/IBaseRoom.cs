using GameServer_Demo.Application.Interfaces;
using GameServer_Demo.Application.Messaging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Room.Interfaces
{
    public interface IBaseRoom
    {
        public string Id { get; set; }
        public ConcurrentDictionary<string,IPlayer> Players { get; set; }

        bool JoinRoom(IPlayer player);
        bool ExitRoom(IPlayer player);
        bool ExitRoom(string Id);

        IPlayer FindPlayer(string id);
        void SendMessage(string mes);
        void SendMessage<T>(WsMessage<T> mes);
        void SendMessage<T>(WsMessage<T> mes, string idIgnore);
    }
}
