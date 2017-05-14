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
    public class NewsSourcesController : BaseController
    {
        private ILogger<NewsSourcesController> _logger;
        private IMapper _mapper;
        private IStockPortfolioRepository _repo;
        public NewsSourcesController(IStockPortfolioRepository repo,
            ILogger<NewsSourcesController> logger,
            IMapper mapper){
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        [EnableCors("CorsPolicy")]
        [HttpGet]
        public async Task<IActionResult>Get()
        {
            try{
                var sources = await _repo.GetNewsSources();
                if(sources == null ) return NotFound($"News Sources were not found.");
                var newsSourceModels = _mapper.Map<IEnumerable<NewsSourceModel>>(sources);
                return Ok(newsSourceModels);

            }
            catch{}
            return BadRequest();
        }

         [EnableCors("CorsPolicy")]    
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]NewsSourceModel model)
        {
            try
            {

                var source = _mapper.Map<NewsSource>(model);

                if (await _repo.AddNewsSource(source))
                {
                    var newUri = Url.Link("NewsSourceGet", new { sourceId = source.name }); 
                    return Created(newUri, _mapper.Map<NewsSourceModel>(source));
                }
                else
                {
                _logger.LogWarning("Could not save News Source to the database");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Threw exception while saving User news: {ex}");
            }

            return BadRequest();

        }

        

    }
}
