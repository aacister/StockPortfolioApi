using AutoMapper;
using StockPortfolio.Api.Models;
using StockPortfolio.Data.Entities;

namespace StockPortfolio.Api.Converters
{
    public class Converter : Profile
    {
        
        public Converter(){
            CreateMap<Stock, StockModel>()
              .ForMember(s => s.Symbol, 
                opt => opt.MapFrom(stock => stock.symbol))
              .ForMember(s => s.Name,
                opt => opt.MapFrom(stock => stock.name))
              .ForMember(s => s.LastPrice,
                opt => opt.MapFrom(stock => stock.lastPrice))
              .ForMember(s => s.PercentChange,
                opt => opt.MapFrom(stock => stock.percentChange))
              .ForMember(s => s.Open,
                opt => opt.MapFrom(stock => stock.open))
              .ForMember(s => s.High,
                opt => opt.MapFrom(stock => stock.high))
              .ForMember(s => s.Low,
                opt => opt.MapFrom(stock => stock.low))
              .ForMember(s => s.Close,
                opt => opt.MapFrom(stock => stock.close));

             CreateMap<News, NewsModel>()
              .ForMember(n => n.Timestamp,
                opt => opt.MapFrom(news => news.timestamp))
              .ForMember(n => n.Headline,
                opt => opt.MapFrom(news => news.headline))
              .ForMember(n => n.Source,
                opt => opt.MapFrom(news => news.source))
              .ForMember(n => n.Preview,
                opt => opt.MapFrom(news => news.preview))
              .ForMember(n => n.HeadlineUrl,
                opt => opt.MapFrom(news => news.headlineURL));

             CreateMap<Weather, WeatherModel>()
              .ForMember(w => w.condition,
                opt => opt.MapFrom(weather => weather.Condition));

             CreateMap<User, UserModel>()
              .ForMember(u => u.UserName,
                opt => opt.MapFrom(user => user.UserName))
              .ForMember(u => u.Password,
                opt => opt.MapFrom(user => user.Password))
              .ForMember(u => u.FirstName,
                opt => opt.MapFrom(user => user.FirstName))
              .ForMember(u => u.LastName,
                opt => opt.MapFrom(user => user.LastName))
              .ForMember(u => u.Zip,
                opt => opt.MapFrom(user => user.Zip));
            
        }

    }
}