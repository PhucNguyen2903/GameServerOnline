using GameServer_Demo.Game_Tick_Tac_Toe.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Application.Messaging.Contains.Match
{
    public struct PlaceData
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public PixelType PixelType { get; set; }
    }
}
