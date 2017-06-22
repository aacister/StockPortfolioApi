using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;


namespace StockPortfolio.Data.Entities
{
    public class User
    {
        [BsonId]
        public string userName {get; set;}
        public string firstName {get; set;}
        public string lastName {get; set;}
        public string zip {get; set;}
        public byte[] hash { get; set; }
        public byte[] salt { get; set; }
        public IEnumerable<Stock> stocks {get; set;}
        public IEnumerable<NewsSource> newsSources {get; set;}
        
    }
}