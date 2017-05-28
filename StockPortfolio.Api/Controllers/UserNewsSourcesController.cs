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
        var sources = await _repo.GetUserNewsSources(username);

        return Ok(_mapper.Map<IEnumerable<NewsSourceModel>>(sources));
        }

         [EnableCors("CorsPolicy")]   
        [HttpGet("{sourceId}", Name="UserNewsSourceGet")]
        public async Task<IActionResult> Get(string username, string sourceId)
        {
           try{
          
                var sources =  await _repo.GetUserNewsSources(username);
                if(sources == null ) return NotFound($"User news sources for {username} were not found.");

                var source = sources.Where(x => x.id == sourceId).FirstOrDefault<NewsSource>();
                if(source == null ) return NotFound($"User news source {sourceId} was not found.");
                
                return Ok(_mapper.Map<NewsSourceModel>(source));
           }
           catch{

           }
           return BadRequest();
        }


        [EnableCors("CorsPolicy")]
        [Authorize]
        [HttpPost("{sourceId}")]
        public async Task<IActionResult> Post(string username, string sourceId)
        {
            try
            {
                var model = await _repo.GetNewsSourceDataBySourceId(sourceId);
                var source = _mapper.Map<NewsSource>(model);
                if(source==null) return NotFound($"News source {sourceId} for {username} was not found.");
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
        [Authorize]
        [HttpDelete("{sourceId}")]
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