using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZipeCodeConsoleCore.Models
{
	public class ZipeCodeNumber
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public string cep { get; set; }
    }
}

