using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StockPortfolio.Data.Entities;
using StockPortfolio.Data;

namespace StockPortfolio.Data.Context
{
    public class UserContext
    {
        private readonly IMongoDatabase _database = null;

        public UserContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.DbConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.DbName);
        }

        public IMongoCollection<User> Users
        {
            get
            {
                return _database.GetCollection<User>("users");
            }
        }

    }
}