using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace StockPortfolio.Api.Models
{
    public class WeatherModel
    {
        public ConditionText condition {get; set;}
    }

    public class ConditionText
    {
        public string text {get; set;}
    }
}