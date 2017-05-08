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
    public class StockProxy
    {
        private readonly string _url  = null;

        public StockProxy(IOptions<Settings> settings)
        {
            _url = settings.Value.StockUri;
            
        }
        public async Task<Stock> GetStockData(string symbol)
        {

                var url = $"{_url}?symbol={symbol}";
                using(var http = new HttpClient()){
                    var response = await http.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Stock>(result);
                    return data;
                }
        }
    }


}