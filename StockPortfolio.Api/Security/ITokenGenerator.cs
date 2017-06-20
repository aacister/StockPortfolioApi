using System.Threading.Tasks;
using StockPortfolio.Api.Models;

namespace  StockPortfolio.Api
{
    public interface ITokenGenerator
    {
        Task<TokenModel> CreateToken(string username);
    }
}