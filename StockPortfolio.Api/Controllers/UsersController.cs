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
    public class UsersController : BaseController
    {
        private ILogger<UsersController> _logger;
        private IMapper _mapper;
        private IStockPortfolioRepository _repo;
        public UsersController(IStockPortfolioRepository repo,
            ILogger<UsersController> logger,
            IMapper mapper){
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        [EnableCors("CorsPolicy")]  
        [HttpGet]
        public async Task<IActionResult> Get()
        {
           try{

                var users =  await _repo.GetAllUsers();
                return Ok(_mapper.Map<IEnumerable<UserModel>>(users));
           }
           catch{

           }
           return BadRequest();
        }

        [EnableCors("CorsPolicy")]   
        [HttpGet("{username}", Name="UserGet")]
        public async Task<IActionResult> Get(string username)
        {
           try{
               User user = null;
                user =  await _repo.GetUser(username);
                
                if(user == null ) return NotFound($"User {username} was not found.");
                
                return Ok(_mapper.Map<UserModel>(user));
           }
           catch{

           }
           return BadRequest();
        }


        [EnableCors("CorsPolicy")]   
        [Authorize]
        [HttpPut("{username}")]
        public async Task<IActionResult> Put(string username, [FromBody] UserModel model)
        {
            try
            {
                var oldUser = await _repo.GetUser(username);
                if (oldUser == null) return NotFound($"Could not find a user with username of {username}");

                if (await _repo.UpdateUser(username, model.FirstName, model.LastName, model.Zip))
                {
                return Ok(_mapper.Map<UserModel>(oldUser));
                }
            }
            catch (Exception)
            {

            }

            return BadRequest("Couldn't update User");
        }

        [EnableCors("CorsPolicy")]
        [Authorize]
        [HttpDelete("{username}")]
        public async Task<IActionResult> Delete(string username)
        {
            try
            {
                var oldUser = await _repo.GetUser(username);
                if (oldUser == null) return NotFound($"Could not find user with username of {username}");

            if( await _repo.DeleteUser(oldUser.userName))
                {
                return Ok();
                }
            }
            catch (Exception)
            {
            }

            return BadRequest("Could not delete User");
        }

    }
}
