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

        public string symbol {get; set;}
        public string name {get; set;}
        public string logoUrl {get; set;}
   

    }
}