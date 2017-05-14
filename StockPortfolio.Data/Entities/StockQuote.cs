using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StockPortfolio.Data.Entities
{
    [DataContract]
    public class StockQuote
    {
        [DataMember]
        public string symbol {get; set;}
        [DataMember]
        public string name {get; set;}
        [DataMember]
        public double lastPrice {get; set;}

        [DataMember]
        public double netChange {get; set;}
        [DataMember]
        public double percentChange {get; set;}

        [DataMember]
        public double open {get; set;}
        [DataMember]
        public double high {get;set;}
        [DataMember]
        public double low {get;set;}
        [DataMember]
        public double close {get;set;}
        [DataMember]
        public int volume {get; set;}
        
    }
}