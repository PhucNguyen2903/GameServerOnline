using Database.MongoDB.Handlers;
using GameServer_Demo.Application.Interfaces;
using GameServer_Demo.Logger;
using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Application.Handlers
{
    public class WsGameServer : WsServer, IWsGameServer
    {
        public readonly int _port;
        public readonly IPlayerManager PlayerManager;
        private readonly IGameLogger _logger;
        private readonly MongoDb _mongoDb;


        public WsGameServer(IPAddress address, int port, IPlayerManager playerManager, IGameLogger logger, MongoDb mongodb) : base(address, port)
        {
            this._port = port;
            PlayerManager = playerManager;
            _logger = logger;
            _mongoDb = mongodb;
        }

        protected override TcpSession CreateSession()
        {
            //to Handle New Session
            _logger.Info("New Session connected");
            var player = new Player(server:this, _mongoDb.GetDatabase());
            PlayerManager.AddPlayer(player);
            return player;
        }

        protected override void OnError(SocketError error)
        {
            _logger.Error("Server Sesstion error");
            base.OnError(error);
        }

        protected override void OnDisconnected(TcpSession session)
        {
            _logger.Info("Session Disconnected");
            var player = PlayerManager.FindPlayer(session.Id.ToString());
            if (player != null)
            {
                PlayerManager.RemovePlayer(player);
            }
            base.OnDisconnected(session);
        }

        public void SendAll(string mes) 
        {
            this.MulticastText(mes);
        }

        public void RestartGameServer()
        {
            if (this.Restart())
            {
                _logger.Print("Server Ws Restarted ");
            }

        }

        public void StartGameServer()
        {
            if (this.Start())
            {
                _logger.Print("Server Ws started at: " + _port);
            }
        }

        public void StopGameServer()
        {
            this.Stop();
            _logger.Print("Server Ws Stopped ");
        }


    }
}
