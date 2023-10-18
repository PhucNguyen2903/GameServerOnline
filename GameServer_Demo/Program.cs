using GameServer_Demo.Application.Handlers;
using GameServer_Demo.Application.Interfaces;
using GameServer_Demo.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Database.MongoDB.Interfaces;
using Database.MongoDB.Handlers;
using GameServer_Demo.GameModel;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace GameServer_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=))");

            IGameLogger gameLogger = new GameLogger();
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            var mongodb = new MongoDb();
            //var mongoHandlers = new MongoHandler<User>(mongodb.GetDatabase());
            IPlayerManager  playerManager = new PlayerManager(gameLogger);
            var ws = new WsGameServer(IPAddress.Any, port: 8080, playerManager, gameLogger, mongodb);
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
