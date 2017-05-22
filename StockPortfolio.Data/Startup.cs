using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;

namespace StockPortfolio.Data
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json");

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
                options.StockQuotesUri = _config.GetSection("Data:StockQutotesUri").Value;
                options.NewsSourceUri = _config.GetSection("Data:NewsSourceUri").Value;
                options.ArticleUri = _config.GetSection("Data:ArticleUri").Value;
            });

        services.AddSingleton(_config);

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
   public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {

           

            loggerFactory.AddConsole(_config.GetSection("Logging"));
            loggerFactory.AddDebug();

    }
  }
}
