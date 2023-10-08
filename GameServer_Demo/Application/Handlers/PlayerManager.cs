using GameServer_Demo.Application.Interfaces;
using GameServer_Demo.Logger;
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

        public ConcurrentDictionary<string, IPlayer> Players { get; set; } // CCU

        private readonly IGameLogger _logger;

        public PlayerManager(IGameLogger logger)
        {
            Players = new ConcurrentDictionary<string, IPlayer>();
            _logger = logger;
        }

        public void AddPlayer(IPlayer player)
        {
            if (FindPlayer(player) == null)
            {
                Players.TryAdd(player.SesstionId, player);
                _logger.Info($"List Players {Players.Count}");
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
                    _logger.Info($"Remove {Id} Success");
                    _logger.Info($"List Players {Players.Count}");
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
