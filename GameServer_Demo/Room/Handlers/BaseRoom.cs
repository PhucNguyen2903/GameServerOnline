using GameServer_Demo.Application.Handlers;
using GameServer_Demo.Application.Interfaces;
using GameServer_Demo.Application.Messaging;
using GameServer_Demo.Application.Messaging.Contains;
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
        public BaseRoom()
        {
            Id = GameHelper.RandomString(10);
            Players = new ConcurrentDictionary<string, IPlayer>();
        }

        public bool ExitRoom(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public bool ExitRoom(string Id)
        {
            throw new NotImplementedException();
        }

        public IPlayer FindPlayer(string id)
        {
            return Players.FirstOrDefault(p => p.Key.Equals(id)).Value;
        }

        public bool JoinRoom(IPlayer player)
        {
            if (FindPlayer(player.SesstionId) == null)
            {
                if (Players.TryAdd(player.SesstionId, player))
                {
                    this.LobbyInfo();
                    return true;
                }
            }
            return false;
        }

        private void LobbyInfo()
        {
            var lobby = new LobbyInfo()
            {
                Players = Players.Values.Select(p => p.GetUserInfo()).ToList()
            };
            var mess = new WsMessage<LobbyInfo>(WsTags.Looby, lobby);
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
