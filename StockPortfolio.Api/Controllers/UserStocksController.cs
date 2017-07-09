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
     [Route("api/users/{username}/stocks")]
    public class UserStocksController: BaseController
    {
        private ILogger<UsersController> _logger;
        private IMapper _mapper;
        private IStockPortfolioRepository _repo;

        public UserStocksController(IStockPortfolioRepository repo,
            ILogger<UsersController> logger,
            IMapper mapper){
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        [EnableCors("CorsPolicy")]      
        [HttpGet]
        public async Task<IActionResult> Get(string username)
        {
            var stocks = await _repo.GetUserStocks(username);

            return Ok(_mapper.Map<IEnumerable<StockModel>>(stocks));
        }

        [EnableCors("CorsPolicy")]   
        [HttpGet("{symbol}", Name="UserStockGet")]
        public async Task<IActionResult> Get(string username, string symbol)
        {
           try{
          
                var stocks =  await _repo.GetUserStocks(username);
                if(stocks== null ) return NotFound($"User stocks for {username} were not found.");

                var stock = stocks.Where(x => x.symbol == symbol).FirstOrDefault<Stock>();
                if(stock == null ) return NotFound($"User stock {symbol} was not found.");
                
                return Ok(_mapper.Map<StockModel>(stock));
           }
           catch{

           }
           return BadRequest();
        }


        [EnableCors("CorsPolicy")]   
   
        [HttpPost("{symbol}")]
        public async Task<IActionResult> Post(string username, string symbol)
        {
            try
            {
                var model = await _repo.GetStock(symbol);
                var stock = _mapper.Map<Stock>(model);

                if (await _repo.AddUserStock(username, stock.symbol))
                {
                    var newUri = Url.Link("UserStockGet", new { symbol = stock.symbol }); 
                    return Created(newUri, _mapper.Map<StockModel>(stock));
                }
                else
                {
                _logger.LogWarning("Could not save User stock to the database");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Threw exception while saving User stock: {ex}");
            }

            return BadRequest();

        }

        [EnableCors("CorsPolicy")]
        [Authorize]
        [HttpDelete("{symbol}")]
        public async Task<IActionResult> Delete(string username, string symbol)
        {
            try
            {
                if(await _repo.DeleteUserStock(username, symbol))
                    return Ok();
                else
                    return NotFound($"Could not delete user stock {symbol} for {username}");

            }
            catch (Exception)
            {
            }

            return BadRequest("Could not delete user stock");
        }
        
    }
}