using System;
namespace ZipeCodeConsoleCore.Models
{
	public abstract class ZipeCodeBase
    {
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        public virtual string cep { get; set; }
    }
}

