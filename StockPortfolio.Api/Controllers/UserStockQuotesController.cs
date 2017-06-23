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
    [Route("api/users/{username}/stockquotes")]
    public class UserStockQuotesController: BaseController
    {
        private ILogger<UsersController> _logger;
        private IMapper _mapper;
        private IStockPortfolioRepository _repo;

        public UserStockQuotesController(IStockPortfolioRepository repo,
            ILogger<UsersController> logger,
            IMapper mapper){
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        [EnableCors("CorsPolicy")]      
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get(string username)
        {
		try{

        		var quotes = await _repo.GetUserStockQuotes(username);

        		return Ok(_mapper.Map<IEnumerable<StockQuoteModel>>(quotes));
		}
		catch{
		}
		return BadRequest();
        }

       [EnableCors("CorsPolicy")]   
        [Authorize]
        [HttpPost("{symbol}")]
        public async Task<IActionResult> Post(string username, string symbol)
        {
            try
            {
                var stock = await _repo.GetStock(symbol);

                if (await _repo.AddUserStock(username, stock.symbol))
                {
                    var quote = await _repo.GetStockQuote(stock.symbol);
        		    return Ok(_mapper.Map<StockQuoteModel>(quote));
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