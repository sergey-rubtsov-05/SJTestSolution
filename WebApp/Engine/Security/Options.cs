using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebApp.Engine.Security
{
    internal static class Options
    {
        public static readonly string AuthorizationCookieName = "authorization";
        public static readonly string IssuerName = "sjTestIssuer";
        public static readonly string AudienceName = "sjTestAudience";
        public static readonly string UsernameHeaderName = "username";
        public static readonly string UsernameClaimType = "http://sjtext.example.local/claims/username";

        public static readonly SymmetricSecurityKey SecurityKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("f4470390-82ba-42fa-a851-4ca94211468d"));
    }
}