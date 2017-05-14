using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockPortfolio.Api.Controllers;
using StockPortfolio.Data.Entities;

namespace StockPortfolio.Api.Models
{
  public class UserUrlResolver : IValueResolver<User, UserModel, string>
  {
    private IHttpContextAccessor _httpContextAccessor;

    public UserUrlResolver(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }


    public string Resolve(User source, UserModel destination, string destMember, ResolutionContext context)
    {
      var url = (IUrlHelper)_httpContextAccessor.HttpContext.Items[BaseController.URLHELPER];
      return url.Link("UserGet", new { username = source.userName });

    }
  }
}
