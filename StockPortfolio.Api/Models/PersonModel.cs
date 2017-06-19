using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace StockPortfolio.Api.Models
{
    public class PersonModel
    {
        
        [Required]
        public string UserName {get; set;}
        [Required]
        public string FirstName {get; set;}
        [Required]
        public string LastName {get; set;}
        public string Zip {get; set;}
        public IList<StockModel> Stocks {get; set;}

        public IList<NewsSourceModel> News {get; set;}

        public byte[] Hash { get; set; }

        public byte[] Salt { get; set; }
    }
}