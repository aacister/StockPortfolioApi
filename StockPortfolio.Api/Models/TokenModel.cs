using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace StockPortfolio.Api.Models
{
    public class TokenModel
    {
        public string AccessToken {get; set;}
        public int ExpiresIn {get; set;}

        
    }
}