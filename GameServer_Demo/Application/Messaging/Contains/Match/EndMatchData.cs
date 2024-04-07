using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Application.Messaging.Contains.Match
{
    public struct EndMatchData
    {
        public string WinnerId { get; set; }
        public int Point { get; set; }
    }
}
