namespace Carvana.MarketExpansion.WebApi.Settings
{
    public class SecuritySettings : ISecuritySettings
    {
        public int AccessTokenLifespanSeconds => 1200;
        public int RefreshTokenLifespanMinutes => 20160;
    }
}