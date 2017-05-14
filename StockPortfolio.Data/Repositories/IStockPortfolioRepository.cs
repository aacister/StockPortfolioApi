using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using StockPortfolio.Data.Entities;

namespace StockPortfolio.Data.Interfaces
{
    public interface IStockPortfolioRepository
    {

        //Stock Quotes
        Task<StockQuote> GetStockQuote(string symbol);

        //Stocks
        Task<IEnumerable<Stock>> GetAllStocks();
        Task<bool> AddStock(Stock stock);

        //News Sources
        Task<IEnumerable<NewsSource>> GetSeedNewsSources();
        Task<IEnumerable<NewsSource>> GetNewsSources();
        Task<bool> AddNewsSource(NewsSource newsSource);
   


        //User Resource
        Task<bool> AddUser(User user);
        Task<bool> DeleteUser(string username);
        Task<bool> UpdateUser(string username, string first, string last, string zip);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUser(string username);

        //User News Source
        Task<IEnumerable<NewsSource>> GetUserNewsSources(string username);
        Task<bool> AddUserNewsSource(string username, NewsSource source);
        Task<bool> DeleteUserNewsSource(string username, string sourceId);
        //User Arcticles
        Task<IEnumerable<Article>> GetUserArticles(string username, string sourceId);
       
       //User Stocks
        Task<IEnumerable<Stock>> GetUserStocks(string username);
        Task<bool> AddUserStock(string username, string symbol);
        Task<bool> DeleteUserStock(string username, string symbol);

    }
}