using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StockPortfolio.Data.Entities
{
    [DataContract]
    public class News
    {
        [DataMember]
        public int newsId{get; set; }
        [DataMember]
        public DateTime timestamp {get; set; }

        [DataMember]
        public string source {get; set; }

        [DataMember]
        public string headline {get;set;}

        [DataMember]
        public string preview {get;set;}

        [DataMember]
        public string headlineURL {get;}
  
    }
}