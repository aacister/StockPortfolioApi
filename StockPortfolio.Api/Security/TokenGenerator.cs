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

namespace StockPortfolio.Api
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly TokenGeneratorOptions _options;
        private readonly ILogger _logger;
        private IStockPortfolioRepository _repo;
        private IMapper _mapper;
        TokenGenerator(IOptions<TokenGeneratorOptions> options,
            ILoggerFactory loggerFactory,
            IStockPortfolioRepository repo,
            IMapper mapper){
                _logger = loggerFactory.CreateLogger<TokenGenerator>();
                _options = options.Value;
                _repo = repo;
                _mapper = mapper;
        }
        public async Task<TokenModel> CreateToken(string username)
        {
            try
            {
                var user = await _repo.GetUser(username);
                var userModel= _mapper.Map<UserModel>(user);
                var now = DateTime.UtcNow;
        
                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userModel.UserName),
                    new Claim(JwtRegisteredClaimNames.Sub, userModel.FirstName),
                    new Claim(JwtRegisteredClaimNames.Sub, userModel.LastName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(now).ToString(), ClaimValueTypes.Integer64)
                };


                var jwt = new JwtSecurityToken(
                    issuer: _options.Issuer,
                    audience: _options.Audience,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(_options.Expiration),
                    signingCredentials: _options.SigningCredentials);
                
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        
                return new TokenModel()
                {
                    AccessToken = encodedJwt,
                    ExpiresIn = (int)_options.Expiration.TotalSeconds
                };    

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while creating JWT: {ex}");
            }
            return null;

        }

        private static long ToUnixEpochDate(DateTime date) => new DateTimeOffset(date).ToUniversalTime().ToUnixTimeSeconds();
    }
}