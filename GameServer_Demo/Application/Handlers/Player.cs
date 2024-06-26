﻿using Database.MongoDB.Handlers;
using Database.MongoDB.Interfaces;
using GameServer_Demo.Application.Interfaces;
using GameServer_Demo.Application.Messaging;
using GameServer_Demo.Application.Messaging.Contains;
using GameServer_Demo.GameModel;
using GameServer_Demo.GameModel.Handlers;
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
        private UserHandlers _userDb { get; set; }

        private User _userInfo { get; set; }
        //private IGameDB<User> _userDb { get; set; }

        public Player(WsServer server, IMongoDatabase database) : base(server)
        {
            SesstionId = this.Id.ToString();
            isDisconnected = false;
            _logger = new GameLogger();
            _userDb = new UserHandlers(database);
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
                        _userInfo = _userDb.FindByUserName(loginData.Username);
                        if (_userInfo != null)
                        {
                            var hashPass = GameHelper.HashPassword(loginData.Password);
                            if (hashPass == _userInfo.Password)
                            {
                                // todo move user to lobby
                                var messInfo = new WsMessage<UserInfo>(WsTags.UserInfo,this.GetUserInfo());
                                this.SendMessage(messInfo);
                                this.PlayerJoinLobby();
                                return;
                            }
                        }
                        var invalidMess = new WsMessage<string>(WsTags.Invanlid, "User or Password is Invalid");
                        this.SendMessage(GameHelper.ParseString(invalidMess));
                        //var user = new User("codephui", "123456", "Admin");
                        //var x = 10;
                        //var newUser = _userDb.Create(user);
                        Console.WriteLine($"Player Test Login Successfully");
                        break;
                    case WsTags.Register:
                        var regisData = GameHelper.ParseStruct<RegisterData>(wsMessage.Data.ToString());

                        if (_userInfo != null)
                        {
                            invalidMess = new WsMessage<string>(WsTags.Invanlid, "You are logined");
                            this.SendMessage(GameHelper.ParseString(invalidMess));
                            return;
                        }

                        var check = _userDb.FindByUserName(regisData.UserName);
                        if (check != null)
                        {
                            invalidMess = new WsMessage<string>(WsTags.Invanlid, "User Registered");
                            this.SendMessage(GameHelper.ParseString(invalidMess));
                            return;
                        }

                       
                        var newUser = new User(regisData.UserName, regisData.Password, regisData.DisPlayName);
                        _userInfo = _userDb.Create(newUser);

                        if (_userInfo != null)
                        {
                            this.PlayerJoinLobby();
                        }
                        break;
                    case WsTags.RoomInfo:
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

        private void PlayerJoinLobby() 
        {
            var lobby = ((WsGameServer)Server).RoomManager.Lobby;
            lobby.JoinRoom(this);
            Console.WriteLine("Player Join Lobby");
        }

        public void SetDisconnection(bool value)
        {
            this.isDisconnected = value;
        }

        public bool SendMessage(string mes)
        {
            return this.SendTextAsync(mes);
        }

        public bool SendMessage<T>(WsMessage<T> mes)
        {
            var mesSend = GameHelper.ParseString(mes);
            return this.SendMessage(mesSend);
        }

        public void OnDisconection()
        {
            // to do logic Handle Player Disconnected
            var lobby = ((WsGameServer)Server).RoomManager.Lobby;
            lobby.ExitRoom(this);
            _logger.Warning("Player Disconnected", null);
        }

        public UserInfo GetUserInfo()
        {
            if (_userInfo != null)
            {
                return new UserInfo()
                {
                    DisplayName = _userInfo.DisplayName,
                    Amount = _userInfo.Amount,
                    Avatar = _userInfo.Avatar,
                    Level = _userInfo.Level,
                };
            }
            return new UserInfo();
        }
    }
}
