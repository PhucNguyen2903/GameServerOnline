using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Application.Messaging.Contains.Match
{
    public struct TurnData
    {
        public string Id { get; set; }
        public string PlayerId { get; set; }
        public int TimerCount { get; set; }
        public int Turn { get; set; }

    }
}
