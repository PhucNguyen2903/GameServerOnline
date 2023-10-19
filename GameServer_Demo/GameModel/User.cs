using GameServer_Demo.Application.Handlers;
using GameServer_Demo.GameModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.GameModel
{
    public class User : BaseModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public int Level { get; set; }
        public long Amount { get; set; }

        public User(string userName, string password, string displayName)
        {
            UserName = userName;
            Password = GameHelper.HashPassword(password);
            DisplayName = displayName;
            Avatar = "";
            Level = 1;
            Amount = 0;
        }
    }
}
