using GameServer_Demo.Application.Interfaces;
using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Application.Handlers
{
    public class Player : WsSession, IPlayer
    {
        public string SesstionId { get; set; }
        public string Name { get; set; }
        private bool isDisconnected { get; set; }

        public Player(WsServer server) : base(server)
        {
            SesstionId = this.Id.ToString();
            isDisconnected = false;
        }

        public override void OnWsConnected(HttpRequest request)
        {
            Console.WriteLine("Player Connected");
            var url = request.Url;
            Console.WriteLine(url);

            isDisconnected = false;
        }

        public override void OnWsDisconnected()
        {
            OnDisconnected();
            base.OnWsDisconnected();
        }

        public override void OnWsReceived(byte[] buffer, long offset, long size)
        {
            var mess = Encoding.UTF8.GetString(buffer, index: (int)offset, count: (int)size);
            Console.WriteLine($"Client {SesstionId} send message {mess}");
            ((WsGameServer)Server).SendAll(mes: $"{this.SesstionId} send message {mess}");
        }

        public void SetDisconnection(bool value)
        {
            this.isDisconnected = value;
        }

        public bool SendMessage(string mes)
        {
            return this.SendTextAsync(mes);
        }

        public void OnDisconection()
        {
            // to do logic Handle Player Disconnected
            Console.WriteLine("Player Disconnected");
        }


    }
}
