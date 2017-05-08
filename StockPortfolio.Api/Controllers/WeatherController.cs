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
    public class WeatherController : BaseController
    {
        private ILogger<WeatherController> _logger;
        private IMapper _mapper;
        private IStockPortfolioRepository _repo;
        public WeatherController(IStockPortfolioRepository repo,
            ILogger<WeatherController> logger,
            IMapper mapper){
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        [EnableCors("CorsPolicy")]   
        [HttpGet("{zip}", Name="WeatherGet")]
        public async Task<IActionResult> Get(long zip)
        {
           try{
               Weather weather = null;
                weather =  await _repo.GetWeatherCondition(zip);
                
                if(weather == null ) return NotFound($"Weather for {zip} was not found.");
                
                return Ok(_mapper.Map<WeatherModel>(weather));
           }
           catch{

           }
           return BadRequest();
        }

    }
}
