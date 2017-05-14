using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StockPortfolio.Data.Entities
{
    [DataContract]
    public class NewsSource
    {
        [DataMember]
        public string id {get; set;}
        [DataMember]
        public string name {get; set;}
        [DataMember]
        public string description {get; set;}
        
    }
}