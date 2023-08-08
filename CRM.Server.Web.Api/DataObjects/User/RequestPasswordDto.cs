using System.ComponentModel.DataAnnotations;

namespace CRM.Server.Web.Api.DataObjects.User
{
    public class RequestPasswordDto
    {
        [Required][EmailAddress]
        public string Email { get; set; }
    }
}
