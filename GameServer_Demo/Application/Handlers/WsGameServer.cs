using GameServer_Demo.Application.Interfaces;
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

        public WsGameServer(IPAddress address, int port, IPlayerManager playerManager) : base(address, port)
        {
            this._port = port;
            PlayerManager = playerManager;
        }
        protected override TcpSession CreateSession()
        {
            //to Handle New Session
            Console.WriteLine("New Session connected");
            var player = new Player(server:this);
            PlayerManager.AddPlayer(player);
            return player;
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine("Server Sesstion error");
            base.OnError(error);
        }

        protected override void OnDisconnected(TcpSession session)
        {
            Console.WriteLine("Session Disconnected");
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

        public void RestartGame()
        {
            this.Restart();
        }

        public void StartGame()
        {
            if (this.Start())
            {
                Console.WriteLine("Server Ws started at: " + _port);
            }
        }

        public void StopGame()
        {
            this.Stop();
        }


    }
}
