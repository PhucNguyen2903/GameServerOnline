﻿using GameServer_Demo.Application.Handlers;
using GameServer_Demo.Application.Interfaces;
using GameServer_Demo.Application.Messaging;
using GameServer_Demo.Application.Messaging.Contains.Match;
using GameServer_Demo.Game_Tick_Tac_Toe.Constant;
using GameServer_Demo.Room.Constant;
using GameServer_Demo.Room.Handlers;
using System.Timers;
using Timer = System.Timers.Timer;

namespace GameServer_Demo.Game_Tick_Tac_Toe.Room
{
    public class TickTacToeRoom : BaseRoom
    {
        private readonly int _time;
        private List<List<int>> Board { get; set; }

        private MatchStatus matchStatus { get; set; }

        private string CurrentTurn { get; set; }

        private Timer TurnTimer { get; set; }

        private string TurnId { get; set; }

        private int Turn { get; set; }

        public TickTacToeRoom(int time = 10) : base(RoomType.Battle)
        {
            _time = time;
            Board = new List<List<int>>();
            matchStatus = MatchStatus.Init;
            this.Turn = 0;
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

            if (this.matchStatus != MatchStatus.Init)
            {
                invalidMess.Data = "The game have been started or ended";
                player.SendMessage(invalidMess);
                return;
            }

            if (this.Players.Count < 2)
            {
                invalidMess.Data = "There must be two people to start the game";
                player.SendMessage(invalidMess);
                return;
            }

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
    }
}