using CRM.Server.Models;
using CRM.Server.Web.Api.Security.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Server.Web.Api.Security
{
    public static class JwtMiddlewareExtension
    {
        public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<JwtMiddleware>();
        }
    }

    public class JwtMiddleware: IMiddleware
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Tokens.TokenOptions _tokenOptions;
        private readonly SigningConfigurations _signingConfigurations;

        public JwtMiddleware(UserManager<ApplicationUser> userManager ,IOptions<Tokens.TokenOptions> tokenOptionsSnapshot, SigningConfigurations signingConfigurations)
        {
            _userManager = userManager;
            _signingConfigurations = signingConfigurations;
            _tokenOptions = tokenOptionsSnapshot.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is object)
            {
                await next(context);
                return;
            }
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
           

            if (token != null)
            {
                var result = await attachUserToContext(context, token);
                if (result == true)
                {
                    await next(context);
                }
                else
                {
                    context.Response.StatusCode = 401; //UnAuthorized
                    await context.Response.WriteAsync("Invalid User Key");
                    return;
                }
            }
            else
            {
                await next(context);
            }
        }

        private dynamic validateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            // var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _signingConfigurations.SecurityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                // return user id from JWT token if validation successful
                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "sub").Value;
                var email = jwtToken.Claims.First(x => x.Type == "email").Value;
                return new { UserId= userId, Email=email };
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

        private async Task<bool> attachUserToContext(HttpContext context, string token)
        {
            var userData = validateToken(token);
            if (userData != null)
            {
                // attach user to context on successful jwt validation
                ApplicationUser userName = await _userManager.FindByIdAsync(userData.UserId);
                if (userName!= null && userName.Email == userData.Email)
                {
                    context.Items["CurrentLoggedUser"] = await _userManager.FindByIdAsync(userData.UserId);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        } 
    }
}