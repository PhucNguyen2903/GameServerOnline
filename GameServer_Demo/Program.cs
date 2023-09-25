using GameServer_Demo.Application.Handlers;
using GameServer_Demo.Application.Interfaces;
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
            IPlayerManager  playerManager = new PlayerManager();
            var ws = new WsGameServer(IPAddress.Any, port: 8080, playerManager);
            ws.StartGame();
            for (; ; )
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.R)
                {
                    Console.WriteLine("Restart");
                    ws.Restart();
                }
                if (key == ConsoleKey.S)
                {
                    Console.WriteLine("Stop");
                    ws.StopGame();
                    break;
                }
            }
        }
    }
}
