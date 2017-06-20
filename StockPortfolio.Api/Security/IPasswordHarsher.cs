namespace StockPortfolio.Api
{
    public interface IPasswordHasher
    {
        byte[] Hash(string password, byte[] salt);
    }
}