using CRM.Server.Web.Api.Core.Services.Communication;
using System.Threading.Tasks;

namespace CRM.Server.Web.Api.Core.Services
{
    public interface IAuthenticationService
    {
        Task<TokenResponse> CreateAccessTokenAsync(string email, string password);
        Task<TokenResponse> RefreshTokenAsync(string refreshToken, string userEmail);
        void RevokeRefreshToken(string refreshToken);
    }
}