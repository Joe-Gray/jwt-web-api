namespace Carvana.MarketExpansion.WebApi.Models
{
    public class LoginTokens
    {
        public LoginTokens(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string AccessToken { get; }
        public string RefreshToken { get; }
    }
}