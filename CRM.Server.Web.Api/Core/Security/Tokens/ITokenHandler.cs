using CRM.Server.Models;
using System.Collections.Generic;

namespace CRM.Server.Web.Api.Core.Security.Tokens
{
    public interface ITokenHandler
    {
         AccessToken CreateAccessToken(ApplicationUser user,List<string> roles, List<System.Security.Claims.Claim> existingPermissionClaims);
         RefreshToken TakeRefreshToken(string token);
         void RevokeRefreshToken(string token);
    }
}