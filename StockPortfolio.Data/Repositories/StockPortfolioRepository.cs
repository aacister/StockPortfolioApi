using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using StockPortfolio.Data;
using StockPortfolio.Data.Interfaces;
using StockPortfolio.Data.Entities;
using StockPortfolio.Data.Context;
using StockPortfolio.Data.Proxy;

namespace StockPortfolio.Data.Repositories
{
    
    public class StockPortfolioRepository: IStockPortfolioRepository
    {
        private readonly UserContext _contextUser= null;
        private readonly StockProxy _proxyStock = null;
        private readonly NewsProxy _proxyNews = null;
        private readonly WeatherProxy _proxyWeather = null;
        public StockPortfolioRepository(IOptions<Settings> settings)
        {
            _contextUser = new UserContext(settings);
            _proxyStock = new StockProxy(settings);
            _proxyNews = new NewsProxy(settings);
            _proxyWeather = new WeatherProxy(settings);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            try{
                return await _contextUser.Users.Find(_ => true).ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        
        public async Task<User> GetUser(string username)
        {
            var filter = Builders<User>.Filter.Eq("UserName", username);
            try{
                return await _contextUser
                                .Users
                                .Find(filter)
                                .FirstOrDefaultAsync();
                                
            }
            catch(Exception ex){
                throw ex;
            }
        }

        public async Task<IEnumerable<News>> GetUserNews(string username)
        {
            var filter = Builders<User>.Filter.Eq("UserName", username);
            try
            {
                var user = await _contextUser    
                        .Users
                        .Find(filter)
                        .FirstOrDefaultAsync();
                
                return user.News;
            }
            catch(Exception ex){
                throw ex;
            }          
        }

        public async Task<IEnumerable<Stock>> GetUserStocks(string username)
        {
            var filter = Builders<User>.Filter.Eq("UserName", username);
            try
            {
                    var filteredUser = await _contextUser
                            .Users
                            .Find(filter)
                            .FirstOrDefaultAsync();

                    return filteredUser.Stocks;
                    
            }
            catch(Exception ex){
                throw ex;
            }
        }

        public async Task<bool> AddUser(User user)
        {
            try
            {
                await _contextUser.Users.InsertOneAsync(user);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return true;
        }

         

        public async Task<bool> DeleteUser(string username){
            try{
                await _contextUser.Users.DeleteOneAsync(
                    Builders<User>.Filter.Eq("UserName", username));
            }
            catch(Exception ex){
                throw ex;
            }
            return true;
        }

        public async Task<bool> UpdateUser(string username, string first, string last, string zip)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UserName, username);
            var update = Builders<User>.Update
                            .Set(u => u.FirstName, first)
                            .Set(u => u.LastName, last)
                            .Set(u => u.Zip, zip);
            try{
                await _contextUser.Users.UpdateOneAsync(filter, update);
              
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public  async Task<Stock> GetStock(string symbol){
            try{
                return await _proxyStock.GetStockData(symbol);
            }
            catch(Exception ex){
                throw ex;
            }
        }

        public async Task<IEnumerable<News>> GetNews(string symbol){
            try{
                return await _proxyNews.GetNewsDataByStockSymbol(symbol);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> AddUserStock(string username, Stock stock)
        {
            var filter = Builders<User>.Filter.Eq("UserName", username);
            var insert = Builders<User>.Update.AddToSet(u => u.Stocks, stock );
        
            try
            {
                    var filteredUser = await _contextUser
                            .Users
                            .UpdateOneAsync(filter, insert);
  
            }
            catch(Exception ex){
                throw ex;
            }
            return true;
        }

        public async Task<bool> AddUserNews(string username, News news)
        {
          
            var filter = Builders<User>.Filter.Eq("UserName", username);
            var insert = Builders<User>.Update.AddToSet(u => u.News, news );
        
            try
            {
                    var filteredUser = await _contextUser
                            .Users
                            .UpdateOneAsync(filter, insert);
  
            }
            catch(Exception ex){
                throw ex;
            }
            return true;
        }

        public async Task<bool> DeleteUserStock(string username, string symbol)
        {
            var stock = await _proxyStock.GetStockData(symbol);// This line will be removed
            //Need to figure out how to pull using symbol
            var filter = Builders<User>.Filter.Eq("UserName", username);
            var delete = Builders<User>.Update.Pull(u => u.Stocks, stock);
            try{
                await _contextUser.Users.UpdateOneAsync(
                    filter, delete);
            }
            catch(Exception ex){
                throw ex;
            }
            return true;
        }

        public async Task<bool> DeleteUserNews(string username, int newsId)
        {
            var news = await _proxyNews.GetNewsDataByNewsId(newsId); // this line will be removed
            //Need to figure out how to Pull using newsId
            var filter = Builders<User>.Filter.Eq("UserName", username);
            var delete = Builders<User>.Update.Pull(u => u.News, news);
            try{
                await _contextUser.Users.UpdateOneAsync(
                    filter, delete);
            }
            catch(Exception ex){
                throw ex;
            }
            return true;
        }

        public async Task<Weather> GetWeatherCondition(long zip)
        {
            try{
                return await _proxyWeather.GetWeatherCondion(zip);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    
        
    }
}