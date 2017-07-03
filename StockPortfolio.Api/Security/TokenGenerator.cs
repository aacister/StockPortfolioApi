using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StockPortfolio.Data.Entities;
using StockPortfolio.Api.Models;
using StockPortfolio.Api.Converters;
using StockPortfolio.Data.Interfaces;
using AutoMapper;

namespace StockPortfolio.Api.Security
{
    public class TokenGenerator : ITokenGenerator
    {
        private IOptions<TokenGeneratorOptions> _options;
        private ILogger<TokenGenerator> _logger;
        private IStockPortfolioRepository _repo;
        private IMapper _mapper;
        public TokenGenerator(IOptions<TokenGeneratorOptions> options,
            ILogger<TokenGenerator> logger,
            IStockPortfolioRepository repo,
            IMapper mapper){
                _options = options;
                _logger = logger;
                _repo = repo;
                _mapper = mapper;
        }
        public async Task<string> CreateToken(string username)
        {
            var token = string.Empty;
            try
            {
                var user = await _repo.GetUser(username);
                var userModel= _mapper.Map<UserModel>(user);
                var now = DateTime.UtcNow;
         
                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userModel.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(now).ToString(), ClaimValueTypes.Integer64)
                };


                var jwt = new JwtSecurityToken(
                    username: username,
                    issuer: _options.Value.Issuer,
                    audience: _options.Value.Audience,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(_options.Value.Expiration),
                    signingCredentials: _options.Value.SigningCredentials);
                
                token = new JwtSecurityTokenHandler().WriteToken(jwt);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while creating JWT: {ex}");
            }

            return token; 

        }

        private static long ToUnixEpochDate(DateTime date) => new DateTimeOffset(date).ToUniversalTime().ToUnixTimeSeconds();
    }
}