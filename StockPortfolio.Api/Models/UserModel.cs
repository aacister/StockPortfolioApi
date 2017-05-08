using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace StockPortfolio.Api.Models
{
    public class UserModel
    {
        public string UserName {get; set;}
        public string Password {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string Zip {get; set;}
        public IList<StockModel> Stocks {get; set;}

        public IList<NewsModel> News {get; set;}
    }
}