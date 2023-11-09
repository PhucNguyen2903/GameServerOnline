using GameServer_Demo.Application.Interfaces;
using GameServer_Demo.Room.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Room.Handlers
{
    public class Lobby: BaseRoom
    {
        public Lobby(RoomType type) : base(type)
        {
            
        }

        public override bool JoinRoom(IPlayer player)
        {
            return base.JoinRoom(player);
        }


    }
}
