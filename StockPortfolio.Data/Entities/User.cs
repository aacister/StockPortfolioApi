using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace StockPortfolio.Data.Entities
{
    public class User
    {
        [BsonId]
        public string Id {get; set;}
        public string UserName {get; set;}
        public string Password {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string Zip {get; set;}
        public IEnumerable<Stock> Stocks {get; set;}
        public IEnumerable<News> News {get; set;}
        
    }
}