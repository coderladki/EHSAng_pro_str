using CRM.Server.Web.Api.Core.Security.Tokens;

namespace CRM.Server.Web.Api.Core.Services.Communication
{
    public class TokenResponse : BaseResponse
    {
        public AccessToken Token { get; set; }

        public long Id { get; set; }

        public TokenResponse(bool success, string message, AccessToken token) : base(success, message)
        {
            Token = token;
        }
    }
}