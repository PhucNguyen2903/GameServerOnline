using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Application.Messaging.Contains
{
    public struct LobbyInfo
    {
      public List<UserInfo> Players { get; set; }
    }
}
