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
        private string _url  = null;
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
            try{
                var url = $"{_url}";
                using(var http = new HttpClient()){
                    var response = await http.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();
                    var responseObj = JsonConvert.DeserializeObject<NewsSourceResponse>(result);

                    if (responseObj.status == "ok")
                        return responseObj.sources;
                    else
                        throw new Exception("Error retrieving data sources.");
                }
            }
            catch(Exception ex){
                throw ex;
            }
        }

        public async Task<NewsSource> GetNewsSourceDataBySourceId(string sourceId)
        {

                var url = $"{_url}"; 
                using(var http = new HttpClient()){
                    var response = await http.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();
                    var responseObj= JsonConvert.DeserializeObject<NewsSourceResponse>(result);
                    if (responseObj.status == "ok")
                        if(responseObj.sources.Count >0)
                        {
                            var sources =  responseObj.sources.Where(x=>x.id.ToLower().Contains(sourceId)).ToList<NewsSource>();
                            if(sources.Count>0)
                                return sources[0];
                            else
                                return null;
                        }
                        else
                            return null;
                    else
                        throw new Exception("Error retrieving data sources.");
                }
        }

       public IMongoCollection<NewsSource> NewsSources
        {
            get
            {
                return _database.GetCollection<NewsSource>("newsSources");
            }
        }
    }


}