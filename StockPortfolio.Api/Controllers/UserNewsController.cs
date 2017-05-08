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
    [Route("api/users/{username}/news")]
    public class UserNewsController: BaseController
    {
        private ILogger<UsersController> _logger;
        private IMapper _mapper;
        private IStockPortfolioRepository _repo;
        public UserNewsController(IStockPortfolioRepository repo,
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
            var news = await _repo.GetUserNews(username);

            return Ok(_mapper.Map<IEnumerable<NewsModel>>(news));
        }

        [EnableCors("CorsPolicy")]   
        [HttpGet("{newsId}", Name="UserNewsGet")]
        public async Task<IActionResult> Get(string username, int newsId)
        {
           try{
          
                var news =  await _repo.GetUserNews(username);
                if(news== null ) return NotFound($"User News for {username} was not found.");

                var newsItem = news.Where(x => x.newsId == newsId);
                if(newsItem == null ) return NotFound($"User News {newsId} was not found.");
                
                return Ok(_mapper.Map<NewsModel>(newsItem));
           }
           catch{

           }
           return BadRequest();
        }


        [EnableCors("CorsPolicy")]    
        [HttpPost]
        public async Task<IActionResult> Post(string username, [FromBody]NewsModel model)
        {
            try
            {

                var news = _mapper.Map<News>(model);

                if (await _repo.AddUserNews(username, news))
                {
                    var newUri = Url.Link("UserNewsGet", new { newsId = news.newsId }); 
                    return Created(newUri, _mapper.Map<NewsModel>(news));
                }
                else
                {
                _logger.LogWarning("Could not save User News to the database");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Threw exception while saving User news: {ex}");
            }

            return BadRequest();

        }

        [EnableCors("CorsPolicy")]
        [HttpDelete("{newsId}")]
        public async Task<IActionResult> Delete(string username, int newsId)
        {
            try
            {
                if(await _repo.DeleteUserNews(username, newsId))
                    return Ok();
                else
                    return NotFound($"Could not delete newsId {newsId} for {username}");

           
            }
            catch (Exception)
            {
            }

            return BadRequest("Could not delete user news");
        }
        
    }
}