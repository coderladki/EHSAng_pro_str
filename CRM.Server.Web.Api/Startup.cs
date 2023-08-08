using AutoMapper;
using CRM.Server.Data;
using CRM.Server.Models;
using CRM.Server.Services.Identity;
using CRM.Server.Web.Api.Core.Security.Hashing;
using CRM.Server.Web.Api.Core.Security.Tokens;
using CRM.Server.Web.Api.Core.Services;
using CRM.Server.Web.Api.Configurations;
using CRM.Server.Web.Api.Security.Hashing;
using CRM.Server.Web.Api.Security.Tokens;
using CRM.Server.Web.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using CRM.Server.Web.Api.Security;
using CRM.Server.Web.Api.Core.JsonConverters;
using CRM.Server.Services;
using Serilog;
using CRM.Server.Models.Configuration;
using Bharuwa.Common.Utilities.Encryption;
using Microsoft.OpenApi.Models;
using CorePush.Apple;
using CorePush.Google;

namespace CRM.Server.Web.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                  .SetBasePath(env.ContentRootPath)
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                  .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                  .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
               builder.SetIsOriginAllowed(_ => true)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials());

                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        );
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 

            AppSettings appSettings = new AppSettings
            {
                Database = Configuration.GetValue<string>("AppSettings:Database"),
                Server = Configuration.GetValue<string>("AppSettings:Server"),
                UserID = Configuration.GetValue<string>("AppSettings:UserID"),
                PassPhrase = Configuration.GetValue<string>("AppSettings:PassPhrase"),
                EncryptedPassword= Configuration.GetValue<string>("AppSettings:Password"),
                Password =  AESEncryption.Decrypt(Configuration.GetValue<string>("AppSettings:PassPhrase"), Configuration.GetValue<string>("AppSettings:Password")),
                LogToFile = Configuration.GetValue<bool>("AppSettings:LogToFile"),
                CentralDatabase = Configuration.GetValue<string>("AppSettings:CentralDatabaseName"),
                AllowProductionQC = Configuration.GetValue<bool>("AppSettings:AllowProductionQC")
            };

            services.AddSingleton<AppSettings>(appSettings);
            GlobalStaticClass.InitiallizeObjects(appSettings);

            services.AddTransient<IUserStore<ApplicationUser>, UserStore>(u => new UserStore(Configuration, appSettings));
            services.AddTransient<IRoleStore<ApplicationRole>, RoleStore>(u => new RoleStore(Configuration, appSettings));
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                //.AddUserManager<ApplicationUserManager>()
                .AddDefaultTokenProviders();


            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                options.JsonSerializerOptions.Converters.Add(new Int32Converter());
                options.JsonSerializerOptions.Converters.Add(new DecimalConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            });

            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<ITokenHandler, Security.Tokens.TokenHandler>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddDomainServices(Configuration, appSettings);
            services.Configure<Security.Tokens.TokenOptions>(Configuration.GetSection("TokenOptions"));
            var tokenOptions = Configuration.GetSection("TokenOptions").Get<Security.Tokens.TokenOptions>();

            var signingConfigurations = new SigningConfigurations(tokenOptions.Secret);
            services.AddSingleton(signingConfigurations);
            services.AddAutoMapper(this.GetType().Assembly);
            services.AddTransient<JwtMiddleware>();
            services.AddDistributedMemoryCache();// TODO: USe redis or SQL Server
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(100);
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
            });

            services.AddScoped<IEmailSender, EmailSender>();
            //services.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();


            services.AddTransient<INotificationService, NotificationService>();
            services.AddHttpClient<FcmSender>();
            services.AddHttpClient<ApnSender>();

            // Configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("FcmNotification");
            services.Configure<FcmNotificationSetting>(appSettingsSection);

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.SaveToken = true;
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,
                    IssuerSigningKey = signingConfigurations.SecurityKey,
                    ClockSkew = TimeSpan.Zero
                };
            });
            //services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            //SerilogConfig.Configure();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/error");
                //app.UseHsts();
            }
            //else
            //{
            //    app.UseExceptionHandler("/error");
            //}
            app.UseCors("CorsPolicy");
            app.UseSession();
            app.UseStaticHttpContext();
            WebHelpers.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            app.UseStaticFiles();
            // Add this line; you'll need `using Serilog;` up the top, too
            // app.UseSerilogRequestLogging(); // not needed right now

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseJwtMiddleware();// or else             app.UseMiddleware<JwtMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "WebApi v1"));

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //      name: "default",
                //      pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
               
                endpoints.MapSwagger();
            });
        }
    }
}