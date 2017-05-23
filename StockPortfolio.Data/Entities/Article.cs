using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StockPortfolio.Data.Entities
{
    [DataContract]
    public class ArticleResponse
    {
        [DataMember]
        public string status {get; set;}

        [DataMember]
        public List<Article> articles {get; set;}
    }
    [DataContract]
    public class Article
    {
        [DataMember]
        public string author{get; set; }
        [DataMember]
        public string title {get; set; }

        [DataMember]
        public string description{get; set; }

        [DataMember]
        public string url {get;set;}
        [DataMember]
        public string publishedAt {get; set;}
    }
}