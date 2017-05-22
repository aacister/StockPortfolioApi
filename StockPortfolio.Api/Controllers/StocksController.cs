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
        [HttpGet("", Name="StocksGet")]
        public async Task<IActionResult>Get()
        {
            try{
                var stocks = await _repo.GetAllStocks();
                if(stocks == null ) {
                    _logger.LogWarning($"Stocks were not found.");
                    return NotFound($"Stocks were not found.");
                    }
                var stockModels = _mapper.Map<IEnumerable<StockModel>>(stocks);
                return Ok(stockModels);

            }
            catch(Exception ex){
                _logger.LogError(ex.Message);
            }
            return BadRequest();
        }
        [EnableCors("CorsPolicy")]
        [HttpGet("{symbol}", Name="StockGet")]
        public async Task<IActionResult>Get(string symbol)
        {
            try{
                var stock = await _repo.GetStock(symbol);
                if(stock == null ){
                    _logger.LogWarning($"Stock was not found.");
                    return NotFound($"Stock was not found.");
                }
                var stockModel = _mapper.Map<StockModel>(stock);
                return Ok(stockModel);

            }
            catch(Exception ex){
                _logger.LogError(ex.Message);
            }
            return BadRequest();
        }

        [EnableCors("CorsPolicy")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]StockModel model)
        {
            try
            {

                var stock = _mapper.Map<Stock>(model);

                if (await _repo.AddStock(stock))
                {
                var newUri = Url.Link("StockGet", new { symbol= stock.symbol});
                return Created(newUri, _mapper.Map<StockModel>(stock));
                }
                else
                {
                    _logger.LogWarning("Could not save Stock to the database");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Threw exception while saving Stock: {ex}");
            }

            return BadRequest();

        }

    }
}
