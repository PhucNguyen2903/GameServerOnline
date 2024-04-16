using GameServer_Demo.Application.Handlers;
using GameServer_Demo.Application.Interfaces;
using GameServer_Demo.Application.Messaging;
using GameServer_Demo.Application.Messaging.Contains.Match;
using GameServer_Demo.Game_Tick_Tac_Toe.Constant;
using GameServer_Demo.Logger;
using GameServer_Demo.Room.Constant;
using GameServer_Demo.Room.Handlers;
using Timer = System.Timers.Timer;

namespace GameServer_Demo.Game_Tick_Tac_Toe.Room
{
    public class TickTacToeRoom : BaseRoom
    {
        private readonly int _time;
        private List<List<PixelType>> Board { get; set; }

        private MatchStatus matchStatus { get; set; }

        private string CurrentTurn { get; set; }

        private Timer TurnTimer { get; set; }

        private string TurnId { get; set; }

        private int Turn { get; set; }

        private readonly IGameLogger _logger;

        public TickTacToeRoom(int time = 20) : base(RoomType.Battle)
        {
            _logger = new GameLogger();
            _time = time;
            Board = new List<List<PixelType>>();
            matchStatus = MatchStatus.Init;
            this.Turn = 0;
            this.InitBoard();
        }

        private void InitBoard()
        {
            this.Board = new List<List<PixelType>>();
            for (int i = 0; i < 10; i++)
            {
                var rows = new List<PixelType>();
                for (int col = 0; col < 10; col++)
                {
                    rows.Add(PixelType.None);
                }
                this.Board.Add(rows);
            }
        }

        private void ResetMatch()
        {
            this.InitBoard();
            this.CurrentTurn = null;
            this.Turn = 0;
            this.matchStatus = MatchStatus.Init;
            this.TurnTimer = null;
            this.TurnId = string.Empty;
        }

        public override bool JoinRoom(IPlayer player)
        {
            if (base.JoinRoom(player))
            {
                if (player.GetUserInfo().Id == this.OwnerId)
                {
                    player.SetPixelType(PixelType.X);
                }
                else
                {
                    player.SetPixelType(PixelType.O);
                }
                this.RoomInfo();
                return true;
            }
            return false;
        }

        public void SetTurn()
        {
            this.Turn += 1;
            this.TurnTimer?.Dispose();
            if (this.CurrentTurn != null)
            {
                var nextPlayer = Players.FirstOrDefault(p => p.Key != CurrentTurn).Value;
                if (nextPlayer != null)
                {
                    this.CurrentTurn = nextPlayer.GetUserInfo().Id;
                    this.TurnId = GameHelper.RandomString(20);
                    var message = new WsMessage<TurnData>(WsTags.Turn, new TurnData
                    {
                        Id = this.TurnId,
                        PlayerId = this.CurrentTurn,
                        TimerCount = _time,
                        Turn = Turn
                    });

                    this.SendMessage(message);

                    this.OnProcessTurn();
                    return;
                }
            }

            var rd = new Random();
            var nextPlayerRd = rd.Next(0, 1000);
            if (nextPlayerRd % 2 == 0)
            {
                var nextPlayer = this.Players.Values.ToList()[0];
                this.CurrentTurn = nextPlayer.GetUserInfo().Id;
            }
            else
            {
                var nextPlayer = this.Players.Values.ToList()[1];
                this.CurrentTurn = nextPlayer.GetUserInfo().Id;
            }
            this.TurnId = GameHelper.RandomString(20);
            var messageTurn = new WsMessage<TurnData>(WsTags.Turn, new TurnData
            {
                Id = this.TurnId,
                PlayerId = this.CurrentTurn,
                TimerCount = _time,
                Turn = Turn
            });

            this.SendMessage(messageTurn);
            this.OnProcessTurn();
        }

        private void OnProcessTurn()
        {
            if (this.TurnTimer != null)
            {
                this.TurnTimer.Stop();
                this.TurnTimer.Dispose();
            }

            this.TurnTimer = new Timer(TimeSpan.FromSeconds(this._time).TotalMilliseconds)
            {
                AutoReset = false
            };
            this.TurnTimer.Elapsed += (sender, args) =>
            {
                this.SetTurn();
            };

            this.TurnTimer?.Start();
        }

        public void OnPlayerSetBlock(LockData data, IPlayer player)
        {
            var invalidMess = new WsMessage<string>(WsTags.Invanlid, "This is not your turn");
            if (data.Id == TurnId && this.CurrentTurn != player.SesstionId)
            {
                player.SendMessage(invalidMess);
                return;
            }

            if (this.matchStatus != MatchStatus.Start)
            {
                invalidMess.Data = "There must be two people to start the game";
                player.SendMessage(invalidMess);
                return;
            }

            //To do check passition of row and col block

            //To do check win
        }

        public void StartGame(IPlayer player)
        {
            var invalidMess = new WsMessage<string>(WsTags.Invanlid, "You don't have permissions");
            if (player.GetUserInfo().Id != OwnerId)
            {
                player.SendMessage(invalidMess);
                return;
            }

            if (this.matchStatus == MatchStatus.Start)
            {
                invalidMess.Data = "The game have been starting";
                player.SendMessage(invalidMess);
                return;
            }

            if (this.Players.Count < 2)
            {
                invalidMess.Data = "There must be two people to start the game";
                player.SendMessage(invalidMess);
                return;
            }

            this.ResetMatch();
            matchStatus = MatchStatus.Start;
            var message = new WsMessage<GameInfo>(WsTags.GameInfo, new GameInfo()
            {
                Status = matchStatus,
                TimeCount = _time,
            });

            this.SendMessage(message);
            this.SetTurn();
        }

        public override bool ExitRoom(IPlayer player)
        {
            base.ExitRoom(player);
            this.RoomInfo();
            return true;
        }

        public void SetPlace(IPlayer player, PlaceData data)
        {
            //check player in matchs

            if (FindPlayer(player.GetUserInfo().Id) == null)
            {
                return;
            }

            //validate turn owner
            var invalidMess = new WsMessage<string>(WsTags.Invanlid, "You don't have permissions");
            if (CurrentTurn != player.GetUserInfo().Id)
            {
                invalidMess.Data = "It's not your turn";
                player.SendMessage(invalidMess);
                return;
            }

            var place = this.Board[data.Row][data.Col];

            if (place != PixelType.None)
            {
                invalidMess.Data = "You can't set Here";
                player.SendMessage(invalidMess);
            }

            this.Board[data.Row][data.Col] = data.PixelType;
            var mess = new WsMessage<PlaceData>(WsTags.SetPlace, data);
            this.SendMessage(mess);
            var gameover = this.CheckGameOver(data);

            if (gameover)
            {
                //to and match
                this.GameOver(player.GetUserInfo().Id);
                return;
            }

            SetTurn();
        }

        private void GameOver(string winerId)
        {
            _logger.Print("Game over");
            matchStatus = MatchStatus.GameOver;
            this.TurnTimer?.Dispose();


            var endMatchData = new EndMatchData()
            {
                WinnerId = winerId,
                Point = 10,
            };
            var mes = new WsMessage<EndMatchData>(WsTags.GameOver, endMatchData);
            this.SendMessage(mes);

        }

        private bool CheckGameOver(PlaceData data)
        {
            return CheckLine(data, 0, 1) || CheckLine(data, 1, 0) || CheckLine(data, 1, 1) || CheckLine(data, 1, -1);
        }
        private bool CheckLine(PlaceData data, int rowDirection, int colDirection)
        {
            int pixelCount = 1;
            int row = data.Row + rowDirection;
            int col = data.Col + colDirection;

            while (row >= 0 && row < 10 && col >= 0 && col < 10 && this.Board[row][col] == data.PixelType)
            {
                pixelCount++;
                row += rowDirection;
                col += colDirection;
            }

            row = data.Row - rowDirection;
            col = data.Col - colDirection;

            while (row >= 0 && row < 10 && col >= 0 && col < 10 && this.Board[row][col] == data.PixelType)
            {
                pixelCount++;
                row -= rowDirection;
                col -= colDirection;
            }

            return pixelCount >= 5;
        }
    }
}

