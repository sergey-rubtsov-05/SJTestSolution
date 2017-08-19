﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;

namespace WebApp.Engine.Security
{
    public class AuthenticateMiddleware : OwinMiddleware
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly UserContext _userContext;

        public AuthenticateMiddleware(OwinMiddleware next, UserContext userContext,
            JwtSecurityTokenHandler jwtSecurityTokenHandler) : base(next)
        {
            _userContext = userContext;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var token = context.Request.Cookies[Options.AuthorizationCookieName];
            if (string.IsNullOrWhiteSpace(token))
            {
                var usernameInHeader = context.Request.Headers[nameof(UserContext.Username)];
                if (string.IsNullOrWhiteSpace(usernameInHeader))
                {
                    await Next.Invoke(context);
                    return;
                }

                token = CreateToken(usernameInHeader);
                context.Response.Cookies.Append(Options.AuthorizationCookieName, token);
            }
            var principal = ValidateToken(token);
            _userContext.Init(principal.Claims);
            context.Authentication.User = principal;

            await Next.Invoke(context);
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
            var claims = new[] { new Claim(nameof(UserContext.Username), usernameInHeader) };
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