using GameServer_Demo.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Application.Interfaces
{
    public interface IMessage<T>
    {
        public WsTags Tags { get; set; }
        public T Data { get; set; }
    }
}
