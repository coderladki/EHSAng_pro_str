
using AutoMapper;
using AutoMapper.Configuration;

namespace CRM.Server.Web.Api.Configurations
{
    public static class AutoMapperConfig
    {
        public static void Configure(MapperConfigurationExpression config)
        {
            config.AllowNullCollections = false;

        }
    }
}