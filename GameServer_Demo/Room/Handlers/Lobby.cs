using GameServer_Demo.Application.Handlers;
using GameServer_Demo.Application.Interfaces;
using GameServer_Demo.Application.Messaging;
using GameServer_Demo.Application.Messaging.Contains;
using GameServer_Demo.Room.Constant;
using GameServer_Demo.Room.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Room.Handlers
{
    public class Lobby : BaseRoom
    {
        private readonly IRoomManager _roomManager;
        public Lobby(RoomType type, IRoomManager roomManager) : base(type)
        {
            this._roomManager = roomManager;
        }

        public override bool JoinRoom(IPlayer player)
        {
            if (base.JoinRoom(player))
            {
                this.RoomInfo();
                this.SendListMatch(player);
                return true;
            }
           
            return false;
        }

        public void SendListMatch(IPlayer player = null)
        {
            var listRoom = this._roomManager.ListRoom();
            var message = new WsMessage<List<RoomInfo>>(WsTags.ListRoom, listRoom.Select(item => item.GetRoomInfo()).ToList());

            if (player != null)
            {
                player.SendMessage(message);
            }
            else
            {
                this.SendMessage(message);
            }
        }
    }
}
