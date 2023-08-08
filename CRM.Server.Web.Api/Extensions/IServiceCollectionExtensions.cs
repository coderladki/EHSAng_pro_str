using CRM.Server.Data;
using CRM.Server.Models.Configuration;
using CRM.Server.Services.Domain;
using EHS.Server.Data;
using EHS.Server.Services.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO.Compression;

namespace CRM.Server.Web.Api
{
    public static class IServiceCollection1Extensions
    {
        public static void AddDomainServices(this IServiceCollection services, IConfiguration configuration, AppSettings appSettings)
        {
            string usersDBConnectionString = "server=" + appSettings.Server + "; user id=" + appSettings.UserID + "; password=" + appSettings.Password + ";initial catalog=" + appSettings.CentralDatabase + ";connection timeout=600;MultipleActiveResultSets=True;"; 
            services.AddScoped<UserService>(s=> new UserService(usersDBConnectionString));
            services.AddScoped<CommonService>(s => new CommonService(usersDBConnectionString));
            services.AddScoped<PermissionsService>(s => new PermissionsService(usersDBConnectionString));
            services.AddScoped<DashboardService>(s => new DashboardService(usersDBConnectionString));
            services.AddScoped<DashboardRepository>(s => new DashboardRepository(usersDBConnectionString));
            services.AddScoped<MasterRepository>(s => new MasterRepository(usersDBConnectionString));
            services.AddScoped<MasterService>(s => new MasterService(usersDBConnectionString));
            services.AddScoped<IncidentService>(s => new IncidentService(usersDBConnectionString));
            services.AddScoped<IncidentRepository>(s => new IncidentRepository(usersDBConnectionString));
                      
        }
    }
}