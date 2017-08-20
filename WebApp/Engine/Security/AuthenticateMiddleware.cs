using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccess;
using DataModel.Enities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Security;

namespace WebApp.Engine.Security
{
    public class AuthenticateMiddleware : OwinMiddleware
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly IUnitOfWork _uow;
        private readonly ISecurityContext _securityContext;

        public AuthenticateMiddleware(OwinMiddleware next, ISecurityContext securityContext,
            JwtSecurityTokenHandler jwtSecurityTokenHandler, IUnitOfWork uow) : base(next)
        {
            _securityContext = securityContext;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
            _uow = uow;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var token = context.Request.Cookies[Options.AuthorizationCookieName];
            if (string.IsNullOrWhiteSpace(token))
            {
                var usernameInHeader = context.Request.Headers[Options.UsernameHeaderName];
                if (string.IsNullOrWhiteSpace(usernameInHeader))
                {
                    await Next.Invoke(context);
                    return;
                }

                CheckUserIsNotExists(usernameInHeader);
                token = CreateToken(usernameInHeader);
                context.Response.Cookies.Append(Options.AuthorizationCookieName, token,
                    new CookieOptions { HttpOnly = true, Expires = DateTime.UtcNow.AddDays(7) });
            }
            var principal = ValidateToken(token);
            _securityContext.Init(principal.Claims);
            context.Authentication.User = principal;

            await Next.Invoke(context);
        }

        private void CheckUserIsNotExists(string usernameInHeader)
        {
            if (_uow.Query<User>().Any(u => u.Name == usernameInHeader))
                throw new AuthenticationException("User already exists");
        }

        private ClaimsPrincipal ValidateToken(string token)
        {
            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = Options.SecurityKey,
                ValidAudience = Options.AudienceName,
                ValidIssuer = Options.IssuerName
            };
            var principal =
                _jwtSecurityTokenHandler.ValidateToken(token, validationParameters, out SecurityToken _);
            return principal;
        }

        private string CreateToken(string usernameInHeader)
        {
            var claims = new[] { new Claim(Options.UsernameClaimType, usernameInHeader) };
            var jwtSecurityToken = new JwtSecurityToken(
                Options.IssuerName,
                Options.AudienceName,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddYears(1),
                claims: claims,
                signingCredentials: new SigningCredentials(Options.SecurityKey, SecurityAlgorithms.HmacSha256));

            var token = _jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
            return token;
        }
    }
}