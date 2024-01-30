﻿using GameServer_Demo.Game_Tick_Tac_Toe.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Application.Messaging.Contains.Match
{
    public struct GameInfo
    {
        public MatchStatus Status { get; set; }

        public int TimeCount { get; set; }

    }
}