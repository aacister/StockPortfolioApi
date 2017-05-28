using AutoMapper;
using StockPortfolio.Api.Models;
using StockPortfolio.Data.Entities;

namespace StockPortfolio.Api.Converters
{
    public class Converter : Profile
    {
        
        public Converter(){
            CreateMap<StockQuote, StockQuoteModel>()
              .ForMember(s => s.Symbol, 
                opt => opt.MapFrom(quote => quote.symbol))
              .ForMember(s => s.Name,
                opt => opt.MapFrom(quote => quote.name))
              .ForMember(s => s.LastPrice,
                opt => opt.MapFrom(quote => quote.lastPrice))
              .ForMember(s => s.PercentChange,
                opt => opt.MapFrom(quote => quote.percentChange))
              .ForMember(s => s.Open,
                opt => opt.MapFrom(quote => quote.open))
              .ForMember(s => s.High,
                opt => opt.MapFrom(quote => quote.high))
              .ForMember(s => s.Low,
                opt => opt.MapFrom(quote => quote.low))
              .ForMember(s => s.Close,
                opt => opt.MapFrom(quote => quote.close))
              .ForMember(s => s.Volume,
                opt => opt.MapFrom(quote => quote.volume))
              .ReverseMap();
              
            CreateMap<Stock, StockModel>()
              .ForMember(s => s.symbol,
                opt => opt.MapFrom(stock =>stock.symbol))
              .ForMember(s => s.name,
                opt => opt.MapFrom(stock => stock.name))
              .ForMember(s => s.logoUrl,
                opt => opt.MapFrom(stock => stock.logoUrl))
              .ReverseMap();

             CreateMap<NewsSource, NewsSourceModel>()
              .ForMember(n => n.id,
                opt => opt.MapFrom(source => source.id))
              .ForMember(n => n.name,
                opt => opt.MapFrom(source => source.name))
                .ForMember(n => n.description,
                opt => opt.MapFrom(source => source.description))
              .ReverseMap();


                CreateMap<Article, ArticleModel>()
                .ForMember(n => n.author,
                opt => opt.MapFrom(article => article.author))
                .ForMember(n => n.title,
                opt => opt.MapFrom(article => article.title))
                .ForMember(n => n.description,
                opt => opt.MapFrom(article => article.description))
                .ForMember(n => n.url,
                opt => opt.MapFrom(article => article.url))
                .ForMember(n => n.publishedAt,
                opt => opt.MapFrom(article => article.publishedAt))
                .ReverseMap();


             CreateMap<User, UserModel>()
              .ForMember(u => u.UserName,
                opt => opt.MapFrom(user => user.userName))
              .ForMember(u => u.Password,
                opt => opt.MapFrom(user => user.password))
              .ForMember(u => u.FirstName,
                opt => opt.MapFrom(user => user.firstName))
              .ForMember(u => u.LastName,
                opt => opt.MapFrom(user => user.lastName))
              .ForMember(u => u.Zip,
                opt => opt.MapFrom(user => user.zip))
                .ReverseMap();

            CreateMap<CredentialModel, UserModel>()
              .ForMember(u => u.UserName,
                opt => opt.MapFrom(cred => cred.UserName))
              .ForMember(u => u.Password,
                opt => opt.MapFrom(cred => cred.Password))
                .ReverseMap();
            
        }

    }
}