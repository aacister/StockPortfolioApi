using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockPortfolio.Data.Entities;
using StockPortfolio.Api.Models;
using StockPortfolio.Api.Converters;
using StockPortfolio.Data.Interfaces;


namespace StockPortfolio.Api.Controllers
{
    [RouteAttribute("api/[controller]")]
    public class StocksController : BaseController
    {
        private ILogger<StocksController> _logger;
        private IMapper _mapper;
        private IStockPortfolioRepository _repo;
        public StocksController(IStockPortfolioRepository repo,
            ILogger<StocksController> logger,
            IMapper mapper){
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        [EnableCors("CorsPolicy")]   
        [HttpGet("{symbol}", Name="StockGet")]
        public async Task<IActionResult> Get(string symbol, bool includeNews = false)
        {
           try{
               Stock stock = null;
               stock =  await _repo.GetStock(symbol);
               if(stock == null ) return NotFound($"Stock {symbol} was not found.");
               
               var stockModel = _mapper.Map<StockModel>(stock);

               if(includeNews)
               {
                     var news = await _repo.GetNews(symbol);
                     if(news != null)
                     {
                        var newsModels = _mapper.Map<IEnumerable<NewsModel>>(news);
                        if(newsModels != null)
                            stockModel.News = newsModels.ToList();
                     }
                }      

                return Ok(stockModel);
           }
           catch{

           }
           return BadRequest();
        }

    }
}
