using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public static string RandomString(int len) 
        {
            var rnd = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid() + $"{DateTime.Now}"));
            return rnd[..len];
        }

        public static string HashPassword(string txt) 
        {
            var crypt = new SHA256Managed();
            var hash = string.Empty;
            var bytes = crypt.ComputeHash(Encoding.UTF8.GetBytes(txt));
            return bytes.Aggregate(hash,(current,thebyte) => current + thebyte.ToString("x2"));
        }

    }
}
