using GameServer_Demo.Game_Tick_Tac_Toe.Constant;

namespace GameServer_Demo.Application.Messaging.Contains.Match
{
    public struct PlaceData
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public PixelType PixelType { get; set; }
    }
}
