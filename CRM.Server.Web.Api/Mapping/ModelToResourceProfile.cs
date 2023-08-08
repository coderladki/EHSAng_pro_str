using System.Linq;
using AutoMapper;
using CRM.Server.Models;
using CRM.Server.Web.Api.Controllers.Resources;
using CRM.Server.Web.Api.Core.Security.Tokens;

namespace CRM.Server.Web.Api.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            //CreateMap<ApplicationUser, UserResource>()
            //    .ForMember(u => u.Roles, opt => opt.MapFrom(u => u.Select(ur => ur)));

            CreateMap<AccessToken, AccessTokenResource>()
                .ForMember(a => a.AccessToken, opt => opt.MapFrom(a => a.Token))
                .ForMember(a => a.RefreshToken, opt => opt.MapFrom(a => a.RefreshToken.Token))
                .ForMember(a => a.Expiration, opt => opt.MapFrom(a => a.Expiration))
                .ForMember(a => a.Roles, opt => opt.MapFrom(a => a.Roles));
        }
    }
}