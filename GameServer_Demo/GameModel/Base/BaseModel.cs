using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameServer_Demo.GameModel.Base
{
    public class BaseModel
    {
        [BsonId]
        [BsonGuidRepresentation((GuidRepresentation)BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public BaseModel() 
        {
            //Id = new BsonObjectId("");
            CreateAt =DateTime.Now;
        }
    }
}
