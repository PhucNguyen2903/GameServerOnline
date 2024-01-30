﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Application.Messaging
{
    public enum WsTags
    {
        Invanlid = 0,
        Login = 1,
        Register = 2,
        UserInfo = 3,
        RoomInfo = 4,
        LobbyInfo = 5,
        CreateRoom = 6,
        QuickPlay = 7,
        ListRoom = 8,
        JoinRoom = 9,
        ExitRoom = 10, 
        StartGame = 11,
        Turn = 12,
        GameInfo = 13
    }
}
