﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Repositories.Entities
{
    public class Courier
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }

        [BsonElement]
        public string Name { get; set; }

        [BsonElement]
        public IList<Package>? Packages { get; set; }
    }
}
