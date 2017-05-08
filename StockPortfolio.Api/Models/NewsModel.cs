using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace StockPortfolio.Api.Models
{
    public class NewsModel
    {
        [Required]
        public int NewsId{get; set;}
        public DateTime Timestamp {get; set;}
        public string Source {get; set;}
        public string Headline {get; set;}
        public string Preview {get; set;}
        public string HeadlineUrl {get; set;}
    }
}