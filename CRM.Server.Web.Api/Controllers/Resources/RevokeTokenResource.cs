using System.ComponentModel.DataAnnotations;

namespace CRM.Server.Web.Api.Controllers.Resources
{
    public class RevokeTokenResource
    {
        [Required]
        public string Token { get; set; }
    }
}