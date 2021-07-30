﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.DTO
{
    public class Game
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public string Publisher { get; set; }
        public string Genre { get; set; }
        public string Platforms { get; set; }
    }
}