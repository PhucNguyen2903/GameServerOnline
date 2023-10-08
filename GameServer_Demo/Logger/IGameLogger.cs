using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Logger
{
    public interface IGameLogger
    {
        void Print(string message);
        void Info(string info);
        void Warning(string ms, Exception exception);
        void Error(string error, Exception exception);
        void Error(string error);
    }
}
