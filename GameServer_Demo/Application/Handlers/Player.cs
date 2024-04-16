
using GameServer_Demo.Application.Interfaces;
using GameServer_Demo.Application.Messaging;
using GameServer_Demo.Application.Messaging.Contains;
using GameServer_Demo.Application.Messaging.Contains.Match;
using GameServer_Demo.Game_Tick_Tac_Toe.Constant;
using GameServer_Demo.Game_Tick_Tac_Toe.Room;
using GameServer_Demo.GameModel;
using GameServer_Demo.GameModel.Handlers;
using GameServer_Demo.Logger;
using MongoDB.Driver;
using NetCoreServer;
using System.Text;

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

        private TickTacToeRoom _currenRoom { get; set; }

        private PixelType PixelType { get; set; }

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
                                var messInfo = new WsMessage<UserInfo>(WsTags.UserInfo, this.GetUserInfo());
                                this.SendMessage(messInfo);
                                this.PlayerJoinLobby();
                                return;
                            }
                        }
                        var invalidMess = new WsMessage<string>(WsTags.Invanlid, "User or Password is Invalid");
                        this.SendMessage(GameHelper.ParseString(invalidMess));

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
                    case WsTags.CreateRoom:
                        var createRoom = GameHelper.ParseStruct<CreateRoomData>(wsMessage.Data.ToString());
                        this.OnUserCreateRoom(createRoom);
                        break;
                    case WsTags.JoinRoom:
                        var roomInfo = GameHelper.ParseStruct<RoomInfoData>(wsMessage.Data.ToString());
                        this.OnUserJoinRoom(roomInfo);
                        break;
                    case WsTags.ExitRoom:
                        this.OnUserExitRoom();
                        break;
                    case WsTags.StartGame:
                        this.OnStartGame();
                        break;
                    case WsTags.Turn:
                        break;
                    case WsTags.SetPlace:
                        this._currenRoom?.SetPlace(this,GameHelper.ParseStruct<PlaceData>(wsMessage.Data.ToString()));
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

        private void OnStartGame()
        {
            if (_currenRoom == null) return;
            _currenRoom.StartGame(this);
        }

        private void OnUserCreateRoom(CreateRoomData data)
        {
            var room = (TickTacToeRoom)((WsGameServer)Server).RoomManager.CreateRoom(data.Time, this._userInfo.Id);
            if (room != null && room.JoinRoom(this))
            {
                var lobby = ((WsGameServer)Server).RoomManager.Lobby;
                lobby.ExitRoom(this);
                lobby.SendListMatch();
                this._currenRoom = room;
            }

            //var messInfo = new WsMessage<UserInfo>(WsTags.CreateRoom, this.GetUserInfo());
            //this.SendMessage(messInfo);
        }

        private void OnUserJoinRoom(RoomInfoData data)
        {
            var room = (TickTacToeRoom)((WsGameServer)Server).RoomManager.FindRoom(data.RoomId);
            if (room != null && room.JoinRoom(this))
            {
                //room.JoinRoom(this);
                this._currenRoom = (TickTacToeRoom)room;
            }
        }

        private void OnUserExitRoom()
        {
            if (_currenRoom == null) return;

            if (this._currenRoom.ExitRoom(this))
            {
                this.PlayerJoinLobby();
            }
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
            this.OnUserExitRoom();
            lobby.ExitRoom(this);
            _logger.Warning("Player Disconnected", null);
        }

        public UserInfo GetUserInfo()
        {
            if (_userInfo != null)
            {
                return new UserInfo()
                {
                    Id = this._userInfo.Id,
                    DisplayName = _userInfo.DisplayName,
                    Amount = _userInfo.Amount,
                    Avatar = _userInfo.Avatar,
                    Level = _userInfo.Level,
                    PixelType = this.PixelType,
                };
            }
            return new UserInfo();
        }

        public void SetPixelType(PixelType type)
        {
            this.PixelType = type;
        }

        public PixelType GetPixelType()
        {
            return this.PixelType;
        }
    }
}
