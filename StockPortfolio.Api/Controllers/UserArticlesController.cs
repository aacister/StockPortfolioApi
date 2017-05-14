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
    [Route("api/users/{username}/newssources/{sourceId}/articles")]
    public class UserArticlesController: BaseController
    {
        private ILogger<UsersController> _logger;
        private IMapper _mapper;
        private IStockPortfolioRepository _repo;
        public UserArticlesController(IStockPortfolioRepository repo,
            ILogger<UsersController> logger,
            IMapper mapper){
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }


        [EnableCors("CorsPolicy")]   
        [HttpGet("")]
        public async Task<IActionResult> Get(string username, string sourceId)
        {
           try{
          
                var articles =  await _repo.GetUserArticles(username, sourceId);
                if(articles== null ) return NotFound($"Articles for source {sourceId}, and user {username}  were not found.");
                
                return Ok(_mapper.Map<IEnumerable<ArticleModel>>(articles));
           }
           catch{

           }
           return BadRequest();
        }


       
        
    }
}