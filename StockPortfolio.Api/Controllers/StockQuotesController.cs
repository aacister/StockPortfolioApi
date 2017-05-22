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
    public class StockQuotesController : BaseController
    {
        private ILogger<StockQuotesController> _logger;
        private IMapper _mapper;
        private IStockPortfolioRepository _repo;
        public StockQuotesController(IStockPortfolioRepository repo,
            ILogger<StockQuotesController> logger,
            IMapper mapper){
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        [EnableCors("CorsPolicy")]   
        [HttpGet("{symbol}", Name="StockQuotesGet")]
        public async Task<IActionResult> Get(string symbol)
        {
           try{
               StockQuote quote = null;
               quote =  await _repo.GetStockQuote(symbol);
               if(quote == null ){
                    _logger.LogWarning($"Stock {symbol} was not found.");
                    return NotFound($"Stock {symbol} was not found.");
               }
               
               var stockQuoteModel = _mapper.Map<StockQuoteModel>(quote); 

                return Ok(stockQuoteModel);
           }
           catch(Exception ex){
                _logger.LogError($"Threw exception while saving Stock: {ex}");
               
           }
           return BadRequest();
        }

      

    }
}
