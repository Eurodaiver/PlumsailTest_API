using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlumsailTest_API
{

    public class ObjectItemInDB
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public List<KeyValuePair<string, object>> props { get; set; }

    }


}
