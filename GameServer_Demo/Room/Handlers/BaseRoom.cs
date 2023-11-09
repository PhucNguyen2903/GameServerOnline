using GameServer_Demo.Application.Handlers;
using GameServer_Demo.Application.Interfaces;
using GameServer_Demo.Application.Messaging;
using GameServer_Demo.Application.Messaging.Contains;
using GameServer_Demo.Room.Constant;
using GameServer_Demo.Room.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Room.Handlers
{
    public class BaseRoom : IBaseRoom
    {
        public string Id { get; set; }
        public ConcurrentDictionary<string, IPlayer> Players { get; set; }
        public RoomType RoomType { get; set; }

        public BaseRoom( RoomType type)
        {
            RoomType = type;
            Id = GameHelper.RandomString(10);
            Players = new ConcurrentDictionary<string, IPlayer>();
        }

        public virtual bool ExitRoom(IPlayer player)
        {
            return this.ExitRoom(player.SesstionId);
        }

        public bool ExitRoom(string Id)
        {
            var player = FindPlayer(Id);
            if (player != null)
            {
                Players.TryRemove(player.SesstionId, out player);
                this.RoomInfo();
                return true;
            }
            return false;
        }

        public virtual IPlayer FindPlayer(string id)
        {
            return Players.FirstOrDefault(p => p.Key.Equals(id)).Value;
        }

        public virtual bool JoinRoom(IPlayer player)
        {
            if (FindPlayer(player.SesstionId) == null)
            {
                if (Players.TryAdd(player.SesstionId, player))
                {
                    this.RoomInfo();
                    return true;
                }
            }
            return false;
        }

        private void RoomInfo()
        {
            var lobby = new RoomInfo()
            {
                //RoomType = RoomType
                Players = Players.Values.Select(p => p.GetUserInfo()).ToList()
            };
            var mess = new WsMessage<RoomInfo>(WsTags.RoomInfo, lobby);
            this.SendMessage(mess);
        }

        public void SendMessage(string mes)
        {
            lock (Players)
            {
                foreach (var player in Players.Values)
                {
                    player.SendMessage(mes);
                }
            }
        }

        public void SendMessage<T>(WsMessage<T> mes)
        {
            lock (Players)
            {
                foreach (var player in Players.Values)
                {
                    player.SendMessage(mes);
                }
            }
        }

        public void SendMessage<T>(WsMessage<T> mes, string idIgnore)
        {
            lock (Players)
            {
                foreach (var player in Players.Values.Where(p => p.SesstionId != idIgnore))
                {
                    player.SendMessage(mes);
                }
            }
        }
    }
}
