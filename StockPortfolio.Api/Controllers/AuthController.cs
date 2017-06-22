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
using  StockPortfolio.Api.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace StockPortfolio.Api.Controllers
{
    [RouteAttribute("api/[controller]")]
    public class AuthController: BaseController
    {
        private ITokenGenerator _tokenGenerator;
        private IPasswordHasher _passwordHasher;
        private ILogger<AuthController> _logger;
        private IStockPortfolioRepository _repo;
        private IMapper _mapper;
        public AuthController(IStockPortfolioRepository repo,
            ILogger<AuthController> logger,
            ITokenGenerator tokenGenerator,
            IPasswordHasher passwordHasher,
            IMapper mapper){
            _repo = repo;
            _logger = logger;
            _tokenGenerator = tokenGenerator;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
  
        }
 
        [EnableCors("CorsPolicy")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CredentialModel credModel)
        {
            try
            {
                var user = await _repo.GetUser(credModel.UserName);
                if (user != null)
                {
                     if (user.hash.SequenceEqual(_passwordHasher.Hash(credModel.Password, user.salt)))
                    {
                        var userModel = _mapper.Map<UserModel>(user);
                        //Create token
                        var token = await _tokenGenerator.CreateToken(userModel.UserName);
                        
                        if(token.Length >0)
                        {
                            userModel.Token = token;
                            return Ok(userModel);
                        }
                        else
                            _logger.LogError("Failed to login.  Failed to create token.");
                    }
                    _logger.LogError("Failed to login.  Password is incorrect.");
                }
                else
                    _logger.LogWarning("Failed to login. User does not exist.");
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while logging in: {ex}");  
            }

            return BadRequest("Failed to login");
        }

        [EnableCors("CorsPolicy")]
        [HttpPost("register")]
        public async Task<IActionResult> Post([FromBody] CredentialModel credModel)
        {
        try
        {
                var user = await _repo.GetUser(credModel.UserName);
                if (user != null)
                {
                    _logger.LogWarning("User already exists.");
                }
                else
                {
                    user = _mapper.Map<User>(credModel);
                    user.salt = CreateSalt();
                    user.hash = _passwordHasher.Hash(credModel.Password, user.salt);
                    user.stocks = new List<Stock>();
                    user.newsSources = new List<NewsSource>();
                    if (await _repo.AddUser(user))
                    {
                        user = await _repo.GetUser(credModel.UserName);
                        var userModel = _mapper.Map<UserModel>(user);
                        //Create Token
                        var token = await _tokenGenerator.CreateToken(credModel.UserName);
                        if(token.Length>0)
                        {
                            userModel.Token= token;
                            return Ok(userModel);  
                        }
                    }
                    else
                        _logger.LogError("Could not register user.");
                }
                return BadRequest("Failed to register");

        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception thrown while registering: {ex}");  
        }

            return BadRequest("Failed to register");
        }

     private byte[] CreateSalt(){
            byte[] salt = new byte[24];
            var keyGenerator = RandomNumberGenerator.Create();
            keyGenerator.GetBytes(salt);
            return salt;
     }

    }
}