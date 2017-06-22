using System.Threading.Tasks;
using StockPortfolio.Api.Models;

namespace  StockPortfolio.Api.Security
{
    public interface ITokenGenerator
    {
        Task<string> CreateToken(string username);
    }
}