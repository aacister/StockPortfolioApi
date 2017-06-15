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
using StockPortfolio.Data;


namespace StockPortfolio.Data.Proxy
{
    public class StockQuoteProxy
    {
        private readonly string _url  = null;

        public StockQuoteProxy(IOptions<Settings> settings)
        {
            _url = settings.Value.StockQuotesUri;

            
        }
	
	public async Task<StockQuote> GetStockQuoteData(string symbol)
        {

                try{
                    var url = $"{_url}&symbols={symbol}";
                    using(var http = new HttpClient()){
                        var response = await http.GetAsync(url);
                        var result = await response.Content.ReadAsStringAsync();
                        var quoteResponse = JsonConvert.DeserializeObject<StockQuoteResponse>(result);
                        if(quoteResponse.status.code == 200)
                            if(quoteResponse.results.Count > 0)
                                return quoteResponse.results[0];
                            else 
                                return null;
                        else
                            throw new Exception(quoteResponse.status.message);
                    }
                }
                catch(Exception ex){
                    throw ex;
                }
        }
	
	public async Task<IEnumerable<StockQuote>> GetStockQuotesData(params string[] symbols)
        {
            try{
		var urlStocks = string.Empty;
		foreach(var symbol in symbols)
		{
			if(urlStocks.length <= 0)
				urlStocks += symbol;
			urlStocks += "%2C" + symbol;
		}
		var url = $"{_url}&symbols={urlStocks}";
                using(var http = new HttpClient()){
                        var response = await http.GetAsync(url);
                        var result = await response.Content.ReadAsStringAsync();
                        var quoteResponse = JsonConvert.DeserializeObject<StockQuoteResponse>(result);
                        if(quoteResponse.status.code == 200)
                            if(quoteResponse.results.Count > 0)
                                return quoteResponse.results[0];
                            else 
                                return null;
                        else
                            throw new Exception(quoteResponse.status.message);
                    }
            }
            catch(Exception ex){
                throw ex;
            }
        }
	
     }
}