using System.Threading.Tasks;

namespace  StockPortfolio.Api
{
    public interface ITokenGenerator
    {
        Task<string> CreateToken(string username);
    }
}