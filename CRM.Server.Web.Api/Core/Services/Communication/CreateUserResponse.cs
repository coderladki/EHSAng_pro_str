using CRM.Server.Models;

namespace CRM.Server.Web.Api.Core.Services.Communication
{
    public class CreateUserResponse : BaseResponse
    {
        public ApplicationUser User { get; private set; }

        public CreateUserResponse(bool success, string message, ApplicationUser user) : base(success, message)
        {
            User = user;
        }
    }
}