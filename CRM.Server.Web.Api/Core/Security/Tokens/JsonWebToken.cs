using System;
using System.Collections.Generic;

namespace CRM.Server.Web.Api.Core.Security.Tokens
{
    public abstract class JsonWebToken
    {
        public string Token { get; protected set; }
        public long Expiration { get; protected set; }

        public List<string> Roles { get; protected set; }

        public List<System.Security.Claims.Claim> ExistingPermissionClaims { get; protected set; }

        public JsonWebToken(string token, long expiration, List<string> roles, List<System.Security.Claims.Claim> existingPermissionClaims)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Invalid token.");


            if (expiration <= 0)
                throw new ArgumentException("Invalid expiration.");

            Token = token;
            Expiration = expiration;
            Roles = roles;
            ExistingPermissionClaims = existingPermissionClaims;
        }

        public bool IsExpired() => DateTime.UtcNow.Ticks > Expiration;

    }
}