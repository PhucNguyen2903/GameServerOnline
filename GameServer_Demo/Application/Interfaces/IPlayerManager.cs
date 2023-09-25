using GameServer_Demo.Application.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Application.Interfaces
{
    public interface IPlayerManager
    {
        ConcurrentDictionary<string, IPlayer> Players { get; set; }

        void AddPlayer(IPlayer player);

        void RemovePlayer(string Id);
        void RemovePlayer(IPlayer Player);

        IPlayer FindPlayer(string Id);
        IPlayer FindPlayer(IPlayer player);

        List<IPlayer> GetPlayer();

    }
}
