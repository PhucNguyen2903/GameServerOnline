﻿using GameServer_Demo.Application.Interfaces;
using GameServer_Demo.Application.Messaging.Contains;
using GameServer_Demo.Game_Tick_Tac_Toe.Room;
using GameServer_Demo.Room.Constant;
using GameServer_Demo.Room.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Room.Handlers
{
    public class RoomManager : IRoomManager
    {   
        public static IRoomManager Instance { get; set; }
        public Lobby Lobby { get; set; }
        private ConcurrentDictionary<string, BaseRoom> Rooms { get; set; }

        public RoomManager()
        {
            Rooms = new ConcurrentDictionary<string, BaseRoom>();
            Lobby = new Lobby(RoomType.Lobby, this);
            Instance = this;
        }
        public BaseRoom FindRoom(string id)
        {
            return Rooms.FirstOrDefault(r => r.Key.Equals(id)).Value;
        }

        public bool RemoveRoom(string Id)
        {
            var oldRoom = FindRoom(Id);
            if (oldRoom != null)
            {
                Rooms.TryRemove(Id, out var room);
                this.Lobby.SendListMatch();
                return room != null;
            }
            return false;
        }

        public BaseRoom CreateRoom( int timer,string ownerId = "")
        {
            var room = new TickTacToeRoom(timer);
            if (!string.IsNullOrEmpty(ownerId))
            {
                room.OwnerId = ownerId;
            }
            Rooms.TryAdd(room.Id, room);
            return room;
        }

        public List<RoomInfo> ListRoom()
        {
            //return Rooms.Values.ToList();
            return null;
        }

        List<BaseRoom> IRoomManager.ListRoom()
        {
            return Rooms.Values.ToList();
        }
    }
}
