using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.Application.Handlers
{
    public static class GameHelper
    {
        public static string ParseString<T>(T Data) 
        {
            return JsonConvert.SerializeObject(Data);
        }

        public static T ParseStruct<T>(string Data)
        {
            return JsonConvert.DeserializeObject<T>(Data);
        }


    }
}
