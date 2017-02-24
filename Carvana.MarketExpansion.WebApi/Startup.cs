using System.Web.Http;
using Carvana.MarketExpansion.WebApi.Attributes;
using Carvana.MarketExpansion.WebApi.Data;
using Carvana.MarketExpansion.WebApi.Services;
using Carvana.MarketExpansion.WebApi.Settings;
using Microsoft.Owin;
using Newtonsoft.Json.Serialization;
using Owin;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;

[assembly: OwinStartup(typeof(Carvana.MarketExpansion.WebApi.Startup))]

namespace Carvana.MarketExpansion.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            ConfigureDependencyResolver(config);
            ConfigureWebApi(app, config);
        }

        private void ConfigureDependencyResolver(HttpConfiguration config)
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            container.Options.DefaultLifestyle = Lifestyle.Transient;

            container.Register<ISecuritySettings, SecuritySettings>(Lifestyle.Transient);
            container.Register<ISqlConnectionFactory, SqlConnectionFactory>(Lifestyle.Transient);
            container.Register<IAccountRepository, AccountRepository>(Lifestyle.Transient);
            container.Register<IJwtEncodingService, JwtEncodingService>(Lifestyle.Transient);
            container.Register<IJwtService, JwtService>(Lifestyle.Transient);
            container.Register<IAccessTokenService, AccessTokenService>(Lifestyle.Transient);
            container.Register<IRefreshTokenService, RefreshTokenService>(Lifestyle.Transient);
            container.Register<IPasswordService, PasswordService>(Lifestyle.Transient);
            container.Register<IAccountService, AccountService>(Lifestyle.Transient);

            container.Verify();

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }

        private void ConfigureWebApi(IAppBuilder app, HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);

            config.Filters.Add(new RequireHttpsAttribute());

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
        }
    }
}
