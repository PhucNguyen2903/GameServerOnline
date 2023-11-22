using GameServer_Demo.Room.Constant;
using GameServer_Demo.Room.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Application.Messaging.Contains
{
    public struct RoomInfo 
    {
        public string RoomId { get; set; }
        public List<UserInfo> Players { get; set; }
      public RoomType RoomType { get; set; }
    }
}
