using System;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DataAccess;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Owin;
using WebApp;

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

            app.UseMiddlewareFromContainer<AuthenticateMiddleware>();
        }
    }

    public class AuthenticateMiddleware : OwinMiddleware
    {
        private readonly SessionContext _sessionContext;

        public AuthenticateMiddleware(OwinMiddleware next, SessionContext sessionContext) : base(next)
        {
            _sessionContext = sessionContext;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var authorizationCookie = context.Request.Cookies["authorization"];
            if (string.IsNullOrWhiteSpace(authorizationCookie))
            {
                var loginHeader = context.Request.Headers["login"];
                if (string.IsNullOrWhiteSpace(loginHeader))
                {
                    await Next.Invoke(context);
                    return;
                }

                authorizationCookie = CreateCookie(loginHeader);
                context.Response.Cookies.Append("authorization", authorizationCookie);
            }
            _sessionContext.Username = DecryptCookie(authorizationCookie);
            context.Authentication.User =
                new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("login", _sessionContext.Username) }, "customCookie"));

            await Next.Invoke(context);
        }

        private string DecryptCookie(string authorizationCookie)
        {
            var securityToken = new JwtSecurityTokenHandler().ReadToken(authorizationCookie);
            return Encoding.UTF8.GetString(Convert.FromBase64String(authorizationCookie));
        }

        private string CreateCookie(string loginHeader)
        {
            var claims = new[] { new Claim("login", loginHeader)};
            var jwtSecurityToken = new JwtSecurityToken(issuer: "sjTest", audience: "sjTestA", notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddYears(1), claims: claims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("hello")),
                    SecurityAlgorithms.HmacSha256));

            var loginHeaderInBase64 = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return loginHeaderInBase64;
        }
    }

    public class SessionContext
    {
        public string Username { get; set; }
    }
}