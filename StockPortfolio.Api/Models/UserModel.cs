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
        [Required]
        public string UserName {get; set;}
        [Required]
        public string Token { get; set; }
        [Required]
        public string FirstName {get; set;}
        [Required]
        public string LastName {get; set;}
        public string Zip {get; set;}
        public IList<StockModel> Stocks {get; set;}

        public IList<NewsSourceModel> News {get; set;}
    

        public UserModel(){
            this.Stocks = new List<StockModel>();
            this.News = new List<NewsSourceModel>();
        }
    }
}