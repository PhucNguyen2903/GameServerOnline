using GameServer_Demo.Game_Tick_Tac_Toe.Constant;

namespace GameServer_Demo.Application.Messaging.Contains
{
    public struct UserInfo
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public int Level { get; set; }
        public long Amount { get; set; }

        public PixelType PixelType { get; set; }
    }
}
