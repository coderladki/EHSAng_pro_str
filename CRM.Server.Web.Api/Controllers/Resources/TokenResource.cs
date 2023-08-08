using System.Collections.Generic;
 
namespace CRM.Server.Web.Api.Controllers.Resources
{
    public class AccessTokenResource
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public long Expiration { get; set; }
        public List<string> Roles { get; set; }
    }
}