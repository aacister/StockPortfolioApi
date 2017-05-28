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
using System.Linq;

namespace StockPortfolio.Data.Repositories
{
    
    public class StockPortfolioRepository: IStockPortfolioRepository
    {
        private readonly UserContext _contextUser= null;
        private readonly StockQuoteProxy _proxyStockQuote = null;
        private readonly NewsSourceProxy _proxyNewsSource = null;
        private readonly ArticleProxy _proxyArticle = null;
        private readonly StockContext _contextStock = null;

        public StockPortfolioRepository(IOptions<Settings> settings)
        {
            _contextUser = new UserContext(settings);
            _proxyStockQuote = new StockQuoteProxy(settings);
            _proxyNewsSource = new NewsSourceProxy(settings);
            _proxyArticle = new ArticleProxy(settings);
            _contextStock = new StockContext(settings);

        }
        //Stocks
        public  async Task<StockQuote> GetStockQuote(string symbol){
            try{
                return await _proxyStockQuote.GetStockQuoteData(symbol);
            }
            catch(Exception ex){
                throw ex;
            }
        }

        //Portfolio Stocks
        public async Task<IEnumerable<Stock>> GetAllStocks()
        {
            try{
                return await _contextStock.Stocks.Find(_ => true).ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Stock> GetStock(string symbol)
        {
            var filter = Builders<Stock>.Filter.Eq("symbol", symbol);
            try{
                return await _contextStock
                                .Stocks
                                .Find(filter)
                                .FirstOrDefaultAsync();
                                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

         public async Task<bool> AddStock(Stock stock)
        {
            try
            {
                stock.symbol = stock.symbol; //Format to upper
                await _contextStock.Stocks.InsertOneAsync(stock);
            
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return true;

        }


        //News Sources
        public async Task<IEnumerable<NewsSource>> GetSeedNewsSources(){
            try{
                return await _proxyNewsSource.GetNewsSourceData();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<NewsSource> GetNewsSourceDataBySourceId(string sourceId)
        {
            try{
                
                return await _proxyNewsSource.GetNewsSourceDataBySourceId(sourceId);
                
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<NewsSource>> GetNewsSources(){
            try{
                return await _proxyNewsSource.NewsSources.Find(_ => true).ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> AddNewsSource(NewsSource newsSource)
        {
            try
            {
                newsSource.id = newsSource.id;
                await _proxyNewsSource.NewsSources.InsertOneAsync(newsSource);
            
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return true;

        }

        //Users
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
                    Builders<User>.Filter.Eq("userName", username));
            }
            catch(Exception ex){
                throw ex;
            }
            return true;
        }

        public async Task<bool> UpdateUser(string username, string first, string last, string zip)
        {
            var filter = Builders<User>.Filter.Eq(u => u.userName, username);
            var update = Builders<User>.Update
                            .Set(u => u.firstName, first)
                            .Set(u => u.lastName, last)
                            .Set(u => u.zip, zip);
            try{
                await _contextUser.Users.UpdateOneAsync(filter, update);
              
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return true;
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
    
            var filter = Builders<User>.Filter.Eq("userName", username);
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

        //User News Source

        public async Task<IEnumerable<NewsSource>> GetUserNewsSources(string username)
        {
            var filter = Builders<User>.Filter.Eq("userName", username);
            try
            {
                var user = await _contextUser    
                        .Users
                        .Find(filter)
                        .FirstOrDefaultAsync();
                
                return user.newsSources;
            }
            catch(Exception ex){
                throw ex;
            }          
        }

        public async Task<bool> AddUserNewsSource(string username, NewsSource source){
            var filter = Builders<User>.Filter.Eq("userName", username);
            var insert = Builders<User>.Update.AddToSet(u => u.newsSources, source );
        
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

        public async Task<bool> DeleteUserNewsSource(string username, string sourceId){
            var source = await _proxyNewsSource.GetNewsSourceDataBySourceId(sourceId);// This line will be removed
            //Need to figure out how to pull using symbol
            var filter = Builders<User>.Filter.Eq("UserName", username);
            var delete = Builders<User>.Update.Pull(u => u.newsSources, source);
            try{
                await _contextUser.Users.UpdateOneAsync(
                    filter, delete);
            }
            catch(Exception ex){
                throw ex;
            }
            return true;
        }

        //User Arcticles
        public async Task<IEnumerable<Article>> GetUserArticles(string username, string sourceId){
            var filter = Builders<User>.Filter.Eq("userName", username);
            try
            {
                    var filteredUser = await _contextUser
                            .Users
                            .Find(filter)
                            .FirstOrDefaultAsync();

                    var sources = filteredUser.newsSources;
                    var source = sources.Where(x => x.id == sourceId).FirstOrDefault<NewsSource>();
                    if(source != null)
                    {
                        return await _proxyArticle.GetArticleData(source.id);
                    }
                    else
                        return null;
                   
                    
            }
            catch(Exception ex){
                throw ex;
            }
        }

        //User Stocks
        public async Task<IEnumerable<Stock>> GetUserStocks(string username)
        {
            var filter = Builders<User>.Filter.Eq("userName", username);
            try
            {
                    var filteredUser = await _contextUser
                            .Users
                            .Find(filter)
                            .FirstOrDefaultAsync();

                    return filteredUser.stocks;
                    
            }
            catch(Exception ex){
                throw ex;
            }
        }

        public async Task<bool> AddUserStock(string username, string symbol)
        {
            var filterStock = Builders<Stock>.Filter.Eq("symbol", symbol);
            var filterUser = Builders<User>.Filter.Eq("userName", username);
            
            try
            {
                    var stock = await _contextStock
                                .Stocks
                                .Find(filterStock)
                                .FirstOrDefaultAsync();
                
                    
                    var insert = Builders<User>.Update.AddToSet(u => u.stocks, stock );           
                    var filteredUser = await _contextUser
                            .Users
                            .UpdateOneAsync(filterUser, insert);
  
            }
            catch(Exception ex){
                throw ex;
            }
            return true;
        }

        public async Task<bool> DeleteUserStock(string username, string symbol)
        {
            var filterStock = Builders<Stock>.Filter.Eq("symbol", symbol);
            var filter = Builders<User>.Filter.Eq("UserName", username);
            
            try{
                var stock = await _contextStock
                                .Stocks
                                .Find(filterStock)
                                .FirstOrDefaultAsync();
                var delete = Builders<User>.Update.Pull(u => u.stocks, stock);
                await _contextUser.Users.UpdateOneAsync(
                    filter, delete);
            }
            catch(Exception ex){
                throw ex;
            }
            return true;
        }

        
        
        



        
    }
}