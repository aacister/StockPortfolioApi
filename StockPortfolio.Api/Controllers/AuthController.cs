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
        private ITokenGenerator _tokenGenerator;
        private readonly IPasswordHasher _passwordHasher;
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
            _tokenProvider = tokenProvider;
            _passwordHasher;
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
                    var personModel = _mapper.Map<PersonModel>(user);
                     if (personModel.Hash.SequenceEqual(_passwordHasher.Hash(model.Password, personModel.Salt)))
                    {

                        var tokenModel = await _tokenGenerator.CreateToken(personModel.UserName);
                        
                        if(tokenModel != null)
                        {
                            var userModel = _mapper.Map<UserModel>(user);
                            userModel.Token = tokenModel;
                            return Ok(userModel);
                        }
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
        public async Task<IActionResult> Register([FromBody] CredentialModel credModel)
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
                if (await _repo.AddUser(user))
                {
                    user = await _repo.GetUser(model.UserName);
                    var personModel = _mapper.Map<PersonModel>(user);
                    if (personModel.Hash.SequenceEqual(_passwordHasher.Hash(model.Password, personModel.Salt)))
                    {

                        var tokenModel = await _tokenGenerator.CreateToken(personModel.UserName);
                        
                        if(tokenModel != null)
                        {
                            var userModel = _mapper.Map<UserModel>(user);
                            userModel.Token = tokenModel;
                            return Ok(userModel);
                        }
                    }
                }
            }

     

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

    }
}