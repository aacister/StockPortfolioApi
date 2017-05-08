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

namespace StockPortfolio.Data.Proxy
{
    public class NewsProxy
    {
        private readonly string _url  = null;

        public NewsProxy(IOptions<Settings> settings)
        {
            _url = settings.Value.NewsUri;
            
        }
        public async Task<IEnumerable<News>> GetNewsDataByStockSymbol(string symbol)
        {

                var url = $"{_url}?symbol={symbol}";
                using(var http = new HttpClient()){
                    var response = await http.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<IEnumerable<News>>(result);
                    return data;
                }
        }

        public async Task<News> GetNewsDataByNewsId(int newsId)
        {

                var url = $"{_url}?newsId={newsId}";
                using(var http = new HttpClient()){
                    var response = await http.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<News>(result);
                    return data;
                }
        }
    }


}