using AutoMapper;
using CRM.Server.Models;
using CRM.Server.Web.Api.Controllers.Resources;

namespace CRM.Server.Web.Api.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<UserCredentialsResource, ApplicationUser>();
        }
    }
}