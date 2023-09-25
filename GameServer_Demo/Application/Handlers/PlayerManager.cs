using GameServer_Demo.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Application.Handlers
{
    internal class PlayerManager : IPlayerManager
    {

        public ConcurrentDictionary<string, IPlayer> Players { get ; set; } // CCU

        public PlayerManager() 
        {
            Players = new ConcurrentDictionary<string, IPlayer> ();
        }

        public void AddPlayer(IPlayer player)
        {
            if (FindPlayer(player) == null)
            {
                Players.TryAdd(player.SesstionId, player);
                Console.WriteLine($"List Players {Players.Count}");
            }
        }

        public IPlayer FindPlayer(string Id)
        {
            return Players.FirstOrDefault(p => p.Key.Equals(Id)).Value;
        }

        public IPlayer FindPlayer(IPlayer player)
        {
            return Players.FirstOrDefault(p => p.Key.Equals(player)).Value;
        }

        public void RemovePlayer(string Id)
        {
            if (FindPlayer(Id) != null)
            {
                Players.TryRemove(Id, out var player);
                if (player != null)
                {
                    Console.WriteLine($"Remove {Id} Success");
                    Console.WriteLine($"List Players {Players.Count}");
                }
            }
        }

        public void RemovePlayer(IPlayer Player)
        {
            this.RemovePlayer(Player.SesstionId);
        }

        public List<IPlayer> GetPlayer() => Players.Values.ToList();
       
    }
}
