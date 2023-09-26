using GameServer_Demo.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Application.Messaging
{
    public class WsMessage<T> : IMessage<T>
    {
        public WsTags Tags { get ; set ; }
        public T Data { get; set; }

        public WsMessage(WsTags tags, T data)
        {
            Tags = tags;
            Data = data;
        }
    }
}
