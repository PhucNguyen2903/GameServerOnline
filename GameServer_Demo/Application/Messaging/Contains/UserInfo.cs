using GameServer_Demo.Game_Tick_Tac_Toe.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Application.Messaging.Contains
{
    public struct UserInfo
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public int Level { get; set; }
        public long Amount { get; set; }

        public PixelType PixelType { get; set; }
    }
}
