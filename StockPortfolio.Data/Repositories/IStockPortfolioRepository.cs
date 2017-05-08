using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using StockPortfolio.Data.Entities;

namespace StockPortfolio.Data.Interfaces
{
    public interface IStockPortfolioRepository
    {
        Task<bool> AddUser(User user);
        Task<bool> DeleteUser(string username);
        Task<bool> UpdateUser(string username, string first, string last, string zip);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUser(string username);
        Task<IEnumerable<News>> GetUserNews(string username);
        Task<bool> AddUserNews(string username, News news);
        Task<bool> DeleteUserNews(string username, int newsId);
        Task<IEnumerable<Stock>> GetUserStocks(string username);
        Task<bool> AddUserStock(string username, Stock stock);
        Task<bool> DeleteUserStock(string username, string symbol);
        Task<Stock> GetStock(string symbol);
        Task<IEnumerable<News>> GetNews(string symbol);
        Task<Weather> GetWeatherCondition(long zip);

    }
}