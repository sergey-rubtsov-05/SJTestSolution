using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DataAccess;
using Microsoft.Owin;
using Owin;
using WebApp;
using WebApp.Engine;
using WebApp.Engine.Security;

[assembly: OwinStartup(typeof(Startup))]

namespace WebApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            app.ConfigureAutofacContainer();

            Database.SetInitializer(new SjInitializer());

            app.Use<ExceptionHandlerMiddleware>();
            app.UseMiddlewareFromContainer<AuthenticateMiddleware>();
        }
    }
}