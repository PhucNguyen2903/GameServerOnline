using Database.MongoDB.Handlers;
using Database.MongoDB.Interfaces;
using GameServer_Demo.Application.Interfaces;
using GameServer_Demo.Application.Messaging;
using GameServer_Demo.Application.Messaging.Contains;
using GameServer_Demo.GameModel;
using GameServer_Demo.Logger;
using MongoDB.Driver;
using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private IGameLogger _logger;
        private IGameDB<User> _userDb { get; set; }

        public Player(WsServer server, IMongoDatabase database) : base(server)
        {
            SesstionId = this.Id.ToString();
            isDisconnected = false;
            _logger = new GameLogger();
            _userDb = new MongoHandler<User>(database);
        }

        public override void OnWsConnected(HttpRequest request)
        {
            _logger.Info("Player Connected");
            var url = request.Url;
            //Console.WriteLine(url);

            isDisconnected = false;
        }

        public override void OnWsDisconnected()
        {
            OnDisconnected();
            base.OnWsDisconnected();
        }

        public override void OnWsReceived(byte[] buffer, long offset, long size)
        {
            _logger.Info("ReceivedData");
            var mess = Encoding.UTF8.GetString(buffer, index: (int)offset, count: (int)size);
            try
            {
                var wsMessage = GameHelper.ParseStruct<WsMessage<object>>(mess);
                switch (wsMessage.Tags)
                {
                    case WsTags.Invanlid:
                        break;
                    case WsTags.Login:
                        var loginData = GameHelper.ParseStruct<LoginData>(wsMessage.Data.ToString());
                        var user = new User("codephui","123456", "Admin");
                        var x = 10;
                        var newUser = _userDb.Create(user);
                        Console.WriteLine($"Player {x} Login Successfully");
                        break;
                    case WsTags.Register:
                        break;
                    case WsTags.Looby:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                //to do send invalid message
                _logger.Error("OnWsReceived error", e);
            }
           // ((WsGameServer)Server).SendAll(mes: $"{this.SesstionId} send message {mess}");
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
            _logger.Warning("Player Disconnected", null);
        }
    }
}
