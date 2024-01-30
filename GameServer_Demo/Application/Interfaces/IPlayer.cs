using GameServer_Demo.Application.Messaging;
using GameServer_Demo.Application.Messaging.Contains;
using GameServer_Demo.Game_Tick_Tac_Toe.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Application.Interfaces
{
    public interface IPlayer
    {
        public string SesstionId { get; set; }
        public string Name { get; set; }
        void SetDisconnection(bool value);
        bool SendMessage(string mes);
        bool SendMessage<T>(WsMessage<T> mes);
        void OnDisconection();

        UserInfo GetUserInfo();

        void SetPixelType(PixelType type);
        PixelType GetPixelType();
    }
}
