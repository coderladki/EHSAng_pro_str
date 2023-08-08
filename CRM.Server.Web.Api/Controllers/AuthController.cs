using AutoMapper;
using CRM.Server.Models;
using CRM.Server.Services;
using CRM.Server.Services.Logger;
using CRM.Server.Web.Api.Controllers.Resources;
using CRM.Server.Web.Api.Core.Security.Tokens;
using CRM.Server.Web.Api.Core.Services;
using CRM.Server.Web.Api.DataObjects.User;
using CRM.Server.Web.Api.DataObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CRM.Server.Web.Api.User.DataObjects;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using AuthorizeAttribute = CRM.Server.Web.Api.Filters.AuthorizeAttribute;
using CRM.Server.Services.Domain;

namespace CRM.Server.Web.Api.Controllers
{
    [ApiController]
    public class AuthController : Controller
    {
        readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        public AuthController(ILogger<UsersController> logger, IMapper mapper, IAuthenticationService authenticationService, UserManager<ApplicationUser> userManager, IEmailSender emailSender, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _mapper = mapper;
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;          
        }

        [Route("/api/auth/login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(UserCredentialsResource userCredentials)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var response = await _authenticationService.CreateAccessTokenAsync(userCredentials.UserName, userCredentials.Password);
                if (!response.Success)
                {
                    //return BadRequest(response.Message);

                    return BadRequest(new
                    {
                        status = "error",
                        message = "Invalid Credentials",
                    });
                }

                var accessTokenResource = _mapper.Map<AccessToken, AccessTokenResource>(response.Token);
                var user = await _userManager.FindByIdAsync(response.Id.ToString());
                var userResponse = new UserResponseDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    CreatedDateTimeUtc = user.CreatedDateTimeUtc,
                    UpdatedDateTimeUtc = user.UpdatedDateTimeUtc
                };                          
                userResponse.Roles = (await _userManager.GetRolesAsync(user)).ToList();
                var existingClaims = (await _userManager.GetClaimsAsync(user).ConfigureAwait(false));
                var permissions = new List<string>();
                var navigationPermissions = new List<string>();
                existingClaims.ToList().ForEach(claim =>
                {
                    if (claim.Type == "Permission")
                    {
                        permissions.Add(claim.Value);
                    }

                    if (claim.Type == "Navigation Permission")
                    {
                        navigationPermissions.Add(claim.Value);
                    }
                });

                return Ok(new
                {
                    token = new
                    {
                        access_token = accessTokenResource.AccessToken,
                        refresh_token = accessTokenResource.RefreshToken,
                        expires_in = accessTokenResource.Expiration,
                        roles = accessTokenResource.Roles,
                        permissions= permissions,
                        navigationPermissions= navigationPermissions
                    },
                    status="ok",
                    message="login Successful",
                    result = new { Permissions = permissions, UserDetails = userResponse, NavigationPermissions = navigationPermissions,Roles= accessTokenResource.Roles },
                    id=response.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace); GlobalStaticClass.SaveAuditLogInFile(JsonConvert.SerializeObject(ex.StackTrace), LogType.Exception);
                return BadRequest(ex.Message);
            }
        }

        [Route("/api/auth/refresh-token")]
        [HttpPost]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenResource refreshTokenResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = await _authenticationService.RefreshTokenAsync(refreshTokenResource.Token, refreshTokenResource.UserEmail);
                if (!response.Success)
                {
                    return BadRequest(response.Message);
                }

                var tokenResource = _mapper.Map<AccessToken, AccessTokenResource>(response.Token);
                return Ok(tokenResource);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace); GlobalStaticClass.SaveAuditLogInFile(JsonConvert.SerializeObject(ex.StackTrace), LogType.Exception);
                return BadRequest();
            }
        }

        [Route("/api/auth/tokenrevoke")]
        [HttpPost]
        public IActionResult RevokeToken(RevokeTokenResource revokeTokenResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _authenticationService.RevokeRefreshToken(revokeTokenResource.Token);
            return NoContent();
        }

        [HttpPost]
        [Authorize]
        [Route("/api/auth/sign-out")]
        public async new Task<IActionResult> SignOut()
        {
            await Task.Run(() => { });
            base.SignOut();
            return Ok();
        }


        //for Android
        [HttpPost]
        [AllowAnonymous]
        [Route("/api/auth/request-pass")]
        public async Task<IActionResult> RequestPassword(RequestPasswordDto requestPasswordDto)
        {
            ApiResponse<string> apiResponse = new ApiResponse<string>
            {
                Status = "",
                Message = "",
                Result = ""
            };
            try
            {
                var user = await _userManager.FindByEmailAsync(requestPasswordDto.Email);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                //var resetLink = Url.Action("reset-pass",
                //                "Auth", new { token = token ,email = user.Email},
                //                 protocol: HttpContext.Request.Scheme);
                var resetLink = $"http://localhost:53761/api/auth/reset-pass?email={user.Email}&token={token}";
                var pathString = "./MailTemplate/ForgotPassword.html";

                var builder = new StringBuilder();

                using (var reader = System.IO.File.OpenText(pathString))
                {
                    builder.Append(reader.ReadToEnd());
                }

                builder.Replace("@resetlink", resetLink);
                builder.Replace("@UserName", user.UserName);
                await _emailSender.SendEmailAsync(requestPasswordDto.Email, "Reset Password", builder.ToString(), "",true);

                apiResponse = new ApiResponse<string>
                {
                    Status = "ok",
                    Message = "registered email",
                    Result = token
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace); GlobalStaticClass.SaveAuditLogInFile(JsonConvert.SerializeObject(ex.StackTrace), LogType.Exception);
                return BadRequest(ex.Message);

            }            

            return Ok(apiResponse);
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[Route("/api/auth/request-pass")]
        //public async Task<IActionResult> RequestPassword(RequestPasswordDto requestPasswordDto)
        //{
        //    try
        //    {
        //        var user = await _userManager.FindByEmailAsync(requestPasswordDto.Email);

        //        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        //        //var resetLink = Url.Action("reset-pass",
        //        //                "Auth", new { token = token ,email = user.Email},
        //        //                 protocol: HttpContext.Request.Scheme);
        //        var resetLink = $"http://localhost:53761/api/auth/reset-pass?email={user.Email}&token={token}";
        //        var pathString = "./MailTemplate/ForgotPassword.html";

        //        var builder = new StringBuilder();

        //        using (var reader = System.IO.File.OpenText(pathString))
        //        {
        //            builder.Append(reader.ReadToEnd());
        //        }

        //        builder.Replace("@resetlink", resetLink);
        //        builder.Replace("@UserName", user.UserName);
        //        await _emailSender.SendEmailAsync(requestPasswordDto.Email, "Reset Password", builder.ToString(), true);

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message, ex.StackTrace); GlobalStaticClass.SaveAuditLogInFile(JsonConvert.SerializeObject(ex.StackTrace), LogType.Exception);
        //        return BadRequest(ex.Message);

        //    }
        //    ApiResponse<dynamic> apiResponse = new ApiResponse<dynamic>
        //    {
        //        Status = "ok",
        //        Message = "Password Reset Link sent to your email id",
        //        Result = ""
        //    };

        //    return Ok(apiResponse);
        //}

        [HttpPost]
        [AllowAnonymous]
        [Route("/api/auth/reset-pass")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequestDto requestDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(requestDto.Email);

                var result = await _userManager.ResetPasswordAsync(user, requestDto.Token, requestDto.Password);

                if (result.Succeeded)
                {

                    return Ok(new { status = "success" });
                }
                else
                {
                    return BadRequest("Error while resetting the password!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace); GlobalStaticClass.SaveAuditLogInFile(JsonConvert.SerializeObject(ex.StackTrace), LogType.Exception);
                return BadRequest(ex.Message);
            }

        }

    }


}