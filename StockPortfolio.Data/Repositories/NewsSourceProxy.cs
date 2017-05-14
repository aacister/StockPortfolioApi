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

namespace StockPortfolio.Data.Proxy
{
    public class NewsSourceProxy
    {
        private readonly string _url  = null;
         private readonly IMongoDatabase _database = null;

        public NewsSourceProxy(IOptions<Settings> settings)
        {
            _url = settings.Value.NewsSourceUri;
            var client = new MongoClient(settings.Value.DbConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.DbName);
            
        }
        public async Task<IEnumerable<NewsSource>> GetNewsSourceData()
        {

                var url = $"{_url}";
                using(var http = new HttpClient()){
                    var response = await http.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<IEnumerable<NewsSource>>(result);
                    return data;
                }
        }

        public async Task<NewsSource> GetNewsSourceDataBySourceId(string sourceId)
        {

                var url = $"{_url}?sourceId={sourceId}";
                using(var http = new HttpClient()){
                    var response = await http.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<NewsSource>(result);
                    return data;
                }
        }

       public IMongoCollection<NewsSource> NewsSources
        {
            get
            {
                return _database.GetCollection<NewsSource>("NewsSource");
            }
        }
    }


}