namespace Carvana.MarketExpansion.WebApi.Settings
{
    public interface ISecuritySettings
    {
        int AccessTokenLifespanSeconds { get; }
        int RefreshTokenLifespanMinutes { get; }
    }
}