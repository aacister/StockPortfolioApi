using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPortfolio.Api.Models
{
    public class StockModel
    {
        [Required]
        public string Symbol {get; set;}
        public string Name {get; set;}
        public double LastPrice {get; set;}
        public double PercentChange {get; set;}
        public double Open {get; set;}
        public double High {get; set;}
        public double Low {get; set;}
        public double Close {get; set;}
        public IList<NewsModel> News {get; set;}

    }
}