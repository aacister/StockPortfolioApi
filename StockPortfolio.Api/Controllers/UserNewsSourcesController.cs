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
    [Route("api/users/{username}/newssources")]
    public class UserNewsSourcesController: BaseController
    {
         private ILogger<UsersController> _logger;
        private IMapper _mapper;
        private IStockPortfolioRepository _repo;
        public UserNewsSourcesController(IStockPortfolioRepository repo,
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
        [HttpGet("{symbol}", Name="UserNewsSourceGet")]
        public async Task<IActionResult> Get(string username, string symbol)
        {
           try{
          
                var stocks =  await _repo.GetUserStocks(username);
                if(stocks== null ) return NotFound($"User stocks for {username} were not found.");

                var stock = stocks.Where(x => x.symbol == symbol);
                if(stock == null ) return NotFound($"User stock {symbol} was not found.");
                
                return Ok(_mapper.Map<StockModel>(stock));
           }
           catch{

           }
           return BadRequest();
        }


        [EnableCors("CorsPolicy")]    
        [HttpPost]
        public async Task<IActionResult> Post(string username, [FromBody]NewsSourceModel model)
        {
            try
            {

                var source = _mapper.Map<NewsSource>(model);

                if (await _repo.AddUserNewsSource(username, source))
                {
                    var newUri = Url.Link("UserNewsSourceGet", new { sourceId = source.name }); 
                    return Created(newUri, _mapper.Map<NewsSourceModel>(source));
                }
                else
                {
                _logger.LogWarning("Could not save news source to the database");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Threw exception while saving news source: {ex}");
            }

            return BadRequest();

        }

        [EnableCors("CorsPolicy")]
        [HttpDelete("{symbol}")]
        public async Task<IActionResult> Delete(string username, string sourceId)
        {
            try
            {
                if(await _repo.DeleteUserNewsSource(username, sourceId))
                    return Ok();
                else
                    return NotFound($"Could not delete user news source {sourceId} for {username}");

            }
            catch (Exception)
            {
            }

            return BadRequest("Could not delete user news source");
        }
        
    }
}