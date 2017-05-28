using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StockPortfolio.Api.Models;
using StockPortfolio.Data;
using StockPortfolio.Data.Entities;
using StockPortfolio.Data.Interfaces;
using StockPortfolio.Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;


namespace StockPortfolio.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            _config = builder.Build();
        }

        IConfigurationRoot _config;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

             services.Configure<Settings>(options =>
            {
                options.DbConnectionString = _config.GetSection("Data:DbConnectionString").Value;
                options.DbName = _config.GetSection("Data:DbName").Value;
                options.StockQuotesUri = _config.GetSection("Data:StockQuotesUri").Value;
                options.NewsSourceUri = _config.GetSection("Data:NewsSourceUri").Value;
                options.ArticleUri = _config.GetSection("Data:ArticleUri").Value;
            });

            var secretKey = _config.GetSection("Tokens:Key").Value;
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            services.Configure<TokenProviderOptions>(options => 
            {
                options.Audience = _config.GetSection("Tokens:Audience").Value;
                options.Issuer = _config.GetSection("Tokens:Issuer").Value;
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

    
            services.AddCors(options => { options.AddPolicy("CorsPolicy", 
                                      builder => builder.AllowAnyOrigin() 
                                                        .AllowAnyMethod() 
                                                        .AllowAnyHeader() 
                                                        .AllowCredentials()); 
                                    });          

            services.AddIdentityWithMongoStores(_config.GetSection("Data:DbConnectionString").Value).AddDefaultTokenProviders();
            services.AddSingleton(_config);
            services.AddAutoMapper();
            services.AddMvc();
            services.AddScoped<IStockPortfolioRepository, StockPortfolioRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors("CorsPolicy"); 

            loggerFactory.AddConsole(_config.GetSection("Logging"));
            loggerFactory.AddDebug();

            var secretKey = _config["Tokens:Key"];
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            //Validate token
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = _config.GetSection("Tokens:Issuer").Value,
                ValidateAudience = true,
                ValidAudience = _config.GetSection("Tokens:Audience").Value,
                ValidateLifetime = true
            };
        
            app.UseJwtBearerAuthentication(new JwtBearerOptions{
                AutomaticAuthenticate = true,
                TokenValidationParameters = tokenValidationParameters
            });

            
 

            app.UseMvc();
        }
    }
}
