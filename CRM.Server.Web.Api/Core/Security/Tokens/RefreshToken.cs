using System.Collections.Generic;

namespace CRM.Server.Web.Api.Core.Security.Tokens
{
    public class RefreshToken : JsonWebRefreshToken
    {
        public RefreshToken(string token, long expiration) : base(token, expiration)
        {
        }
    }
}