using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StockPortfolio.Data.Entities;
using StockPortfolio.Data;
using Newtonsoft.Json;

namespace StockPortfolio.Data.Proxy
{
    public class WeatherProxy
    {
        private readonly string _url  = null;

        public WeatherProxy(IOptions<Settings> settings)
        {
            _url = settings.Value.WeatherUri;
            
        }
    public async Task<Weather> GetWeatherCondion(long zip)
        {

            var url = $"{_url}?zip={zip}";
            using(var http = new HttpClient()){
                var response = await http.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Weather>(result);
                return data;
            }



        }
    }


}