using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace StockPortfolio.Api.Models
{
    public class ArticleModel
    {
        public string author{get; set;}
        public string title{get; set;}
        public string description {get; set;}
        public string url {get; set;}
        public string publishedAt {get; set;}
        public NewsSourceModel source {get; set;}
    }
}