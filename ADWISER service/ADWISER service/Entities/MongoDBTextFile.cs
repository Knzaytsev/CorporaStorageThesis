using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADWISER_service.Models
{
    public class MongoDBTextFile : IText
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Source { get; set; }

        public string TypeText { get; set; }

        public DateTime Date { get; set; }

        public string Annotation { get; set; }

        public string Commentary { get; set; }

        public uint Mark { get; set; }
    }
}
