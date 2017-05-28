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
using System.IdentityModel.Tokens.Jwt;

namespace StockPortfolio.Api.Controllers
{
     [RouteAttribute("api/[controller]")]
    public class AuthController: BaseController
    {
        private TokenProvider _tokenProvider;
        private ILogger<AuthController> _logger;
        private IStockPortfolioRepository _repo;
        private IMapper _mapper;
        public AuthController(IStockPortfolioRepository repo,
            ILogger<AuthController> logger,
            TokenProvider tokenProvider,
            IMapper mapper){
            _repo = repo;
            _logger = logger;
            _tokenProvider = tokenProvider;
            _mapper = mapper;
  
        }

        [EnableCors("CorsPolicy")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] CredentialModel model)
        {
        try
        {
            var user = await _repo.GetUser(model.UserName);
            if (user != null)
            {
                if (VerifyPassword(user, model.Password))
                {
                    var tokenModel = await _tokenProvider.CreateToken(model);
                    if(tokenModel != null)
                        return Ok(tokenModel);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception thrown while logging in: {ex}");  
        }

            return BadRequest("Failed to login");
        }

        [EnableCors("CorsPolicy")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserModel model)
        {
        try
        {
            var user = _mapper.Map<User>(model);

                if (await _repo.AddUser(user))
                {
                   var userModel= _mapper.Map<UserModel>(user);
                   var credentialModel = _mapper.Map<CredentialModel>(userModel);

                   var tokenModel = await _tokenProvider.CreateToken(credentialModel);
                   if(tokenModel != null)
                        return Ok(tokenModel);
                }
                else
                {
                    _logger.LogWarning("Could not register user");
                }

        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception thrown while registering: {ex}");  
        }

            return BadRequest("Failed to register");
        }


        private bool VerifyPassword(User user, string modelPassword){
            return user.password.Trim().ToUpper() == modelPassword.Trim().ToUpper();

        }
    }
}