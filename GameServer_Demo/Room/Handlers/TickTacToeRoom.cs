using GameServer_Demo.Room.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Room.Handlers
{
    public class TickTacToeRoom : BaseRoom
    {
        private readonly int _time;
        public TickTacToeRoom(int time = 10) : base(RoomType.Battle)
        {
            _time = time;
        }
    }
}
