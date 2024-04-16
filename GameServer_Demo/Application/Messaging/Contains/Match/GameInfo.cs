using GameServer_Demo.Game_Tick_Tac_Toe.Constant;

namespace GameServer_Demo.Application.Messaging.Contains.Match
{
    public struct GameInfo
    {
        public MatchStatus Status { get; set; }

        public int TimeCount { get; set; }

    }
}
