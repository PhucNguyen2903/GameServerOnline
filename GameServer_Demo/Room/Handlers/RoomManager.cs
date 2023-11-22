﻿using GameServer_Demo.Application.Messaging.Contains;
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
        public Lobby Lobby { get; set; }
        private ConcurrentDictionary<string, BaseRoom> Rooms { get; set; }

        public RoomManager()
        {
            Rooms = new ConcurrentDictionary<string, BaseRoom>();
            Lobby = new Lobby(RoomType.Lobby, this);
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
                return room != null;
            }
            return false;
        }

        public BaseRoom CreateRoom( int timer)
        {
            var room = new TickTacToeRoom(timer);
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
