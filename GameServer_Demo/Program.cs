using GameServer_Demo.Application.Handlers;
using GameServer_Demo.Application.Interfaces;
using GameServer_Demo.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=))");

            IGameLogger gameLogger = new GameLogger();
            IPlayerManager  playerManager = new PlayerManager(gameLogger);
            var ws = new WsGameServer(IPAddress.Any, port: 8080, playerManager, gameLogger);
            ws.StartGameServer();
            gameLogger.Print("Game Server Started");

            for (; ; )
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.R)
                {
                    gameLogger.Print("Game Server Restartting");
                    ws.RestartGameServer();
                }
                if (key == ConsoleKey.S)
                {
                    gameLogger.Info("Game Server Stopping");
                    ws.StopGameServer();
                    break;
                }
            }
        }
    }
}
