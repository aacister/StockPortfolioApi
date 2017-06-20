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
    public class ArticleProxy
    {
        private readonly string _url  = null;


        public ArticleProxy(IOptions<Settings> settings)
        {
            _url = settings.Value.ArticleUri; 
        }


        public async Task<IEnumerable<Article>> GetArticlesData(params string[] sourceIds)
        {
            try{
                var articles = new List<Article>();
                foreach(var sourceId in sourceIds)
                {
                    var url = $"{_url}{sourceId}";
                    using(var http = new HttpClient())
                    {
                        var response = await http.GetAsync(url);
                        var result = await response.Content.ReadAsStringAsync();
                        var articleResponse = JsonConvert.DeserializeObject<ArticleResponse>(result);
                        if(articleResponse.status == "ok")
                        {
                            foreach(var a in articleResponse.articles)
                                articles.Add(a);
                        }

                    }
                }
                return articles;
                
            }
            catch(Exception ex){
                throw new Exception(ex.Message);
            }
        }
    }

}

