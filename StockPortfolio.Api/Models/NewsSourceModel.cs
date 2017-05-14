using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace StockPortfolio.Api.Models
{
    public class NewsSourceModel
    {
        public string id{get; set;}
        public string name {get; set;}
        public string description {get; set;}

    }
}