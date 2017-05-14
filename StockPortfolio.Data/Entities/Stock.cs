using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace StockPortfolio.Data.Entities
{
    public class Stock
    {

        [BsonId]
        public string id {get; set;}

        public string symbol {get; set;}
        public string name {get; set;}
        public string logoUrl {get; set;}
    }
}