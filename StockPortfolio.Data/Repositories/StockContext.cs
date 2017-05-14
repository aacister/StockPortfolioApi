using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using Microsoft.Extensions.Options;
using System.Text;
using System.Threading.Tasks;
using StockPortfolio.Data.Entities;
using Newtonsoft.Json;
using MongoDB.Driver;
using StockPortfolio.Data;


namespace StockPortfolio.Data.Proxy
{
    public class StockContext
    {

        private readonly IMongoDatabase _database = null;

        public StockContext(IOptions<Settings> settings)
        {

            var client = new MongoClient(settings.Value.DbConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.DbName);
            
        }
         public IMongoCollection<Stock> Stocks
        {
            get
            {
                return _database.GetCollection<Stock>("Stock");
            }
        }
    }


}