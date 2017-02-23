namespace Carvana.MarketExpansion.WebApi.Services
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool IsPasswordValid(string password, string hashedPassword);
    }
}