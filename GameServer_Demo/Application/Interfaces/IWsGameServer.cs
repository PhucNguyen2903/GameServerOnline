﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Application.Interfaces
{
    public interface IWsGameServer
    {
        void StartGameServer();
        void StopGameServer();
        void RestartGameServer();
    }
}
