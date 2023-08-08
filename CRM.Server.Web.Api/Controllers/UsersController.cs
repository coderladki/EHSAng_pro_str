using CRM.Server.Models;
using CRM.Server.Models.Enum;
using CRM.Server.Services;
using CRM.Server.Services.Domain;
using CRM.Server.Web.Api.User.DataObjects;
using CRM.Server.Web.Api.DataObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizeAttribute = CRM.Server.Web.Api.Filters.AuthorizeAttribute;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using Dapper;
using System.Threading;
using EHS.Server.Models.Masters;

namespace CRM.Server.Web.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class UsersController : BaseAuthorizeController
    {
        readonly ILogger<UsersController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserService _userService;
        private readonly IEmailSender _emailSender;
        private IWebHostEnvironment _env;

        public UsersController(ILogger<UsersController> logger, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, UserService userService, IEmailSender emailSender, IWebHostEnvironment env)
        {
            this._logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
            _emailSender = emailSender;
            _env = env;

        }

        [HttpPost]
        [HttpGet]
        [Authorize]
        [Route("/api/users/loggeduser")]
        public async Task<IActionResult> GetLoggedUserAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;
                if (user == null)
                {
                    return NoContent();
                }

                string imagePath = Path.Combine(_env.WebRootPath, "user.png");
                byte[] imageByteArray = null;
                FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    imageByteArray = new byte[reader.BaseStream.Length];
                    for (int i = 0; i < reader.BaseStream.Length; i++)
                        imageByteArray[i] = reader.ReadByte();
                }
                string base64String = "data:image/png;base64," + Convert.ToBase64String(imageByteArray, 0, imageByteArray.Length);
                var userResponse = new UserResponseDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Picture = base64String,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    CreatedDateTimeUtc = user.CreatedDateTimeUtc,
                    UpdatedDateTimeUtc = user.UpdatedDateTimeUtc
                };
                userResponse.Roles = (await _userManager.GetRolesAsync(user)).ToList();

                ApiResponse<UserResponseDto> apiResponse = new ApiResponse<UserResponseDto>("ok", "", userResponse);                
                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return BadRequest();
            }
        }        

        [HttpGet]
        //[Authorize(AuthenticationSchemes = "Bearer")]//, Roles = "Admin")]
        
        [Route("/api/users/get/{id}")]
        public async Task<IActionResult> GetUserAsync(long id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return NoContent();
                }

                var userResponse = new UserResponseDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName=user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    CreatedDateTimeUtc = user.CreatedDateTimeUtc,
                    UpdatedDateTimeUtc = user.UpdatedDateTimeUtc
                };
                userResponse.Roles = (await _userManager.GetRolesAsync(user)).ToList();

                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return BadRequest();

            }
        }

        [HttpGet]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        
        [Route("/api/userss/getAll")]
        public async Task<IActionResult> GetAllUserAsync()
        {
            try
            {
                var users = await _userService.GetAllUserAsync();
                if (users == null)
                {
                    return NoContent();
                }
                var userResponseList = new List<UserResponseDto>();
                foreach (var user in users)
                {
                    userResponseList.Add(new UserResponseDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Roles = (await _userManager.GetRolesAsync(user)).ToList(),
                        Status = user.Status,
                        CreatedDateTimeUtc = user.CreatedDateTimeUtc,
                        UpdatedDateTimeUtc = user.UpdatedDateTimeUtc
                    });

                }
                // var userResponseList = _userManager.Users;

                return Ok(userResponseList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return BadRequest();
            }
        }

        [HttpGet]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        
        [Route("/api/userss/roleid/{roleid}")]
        public async Task<IActionResult> GetAllUserByRoleIdAsync([FromRoute]int roleid)
        {
            try
            {
                // var users = await _userService.GetAllUserByRoleIdAsync(roleid);
                var users = await _userManager.GetUsersInRoleAsync("ProductConsultant").ConfigureAwait(false);
                if (users == null)
                {
                    return NoContent();
                }
                var userResponseList = new List<UserResponseDto>();
                foreach (var user in users)
                {
                    userResponseList.Add(new UserResponseDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Roles = (await _userManager.GetRolesAsync(user)).ToList(),
                        Status = user.Status,
                        CreatedDateTimeUtc = user.CreatedDateTimeUtc,
                        UpdatedDateTimeUtc = user.UpdatedDateTimeUtc
                    });

                }

                return Ok(userResponseList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return BadRequest();
            }
        }
        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer")]//, Roles = "Admin"
        
        [Route("/api/users/create")]
        public async Task<IActionResult> CreateUserAsync([FromBody]CreateUserRequestDto userCredentials, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (String.IsNullOrEmpty(userCredentials.UnitName.ToString()) )
                {
                    throw new Exception("Plant id is missing");
                }
                var password = userCredentials.Password;
                var user = new ApplicationUser
                {
                    FirstName = userCredentials.FirstName,
                    LastName = userCredentials.LastName,
                    UserName = userCredentials.Email,
                    Email = userCredentials.Email,
                    LockoutEnabled = false,
                    TwoFactorEnabled = false,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    AccessFailedCount = 0,
                    PhoneNumber = userCredentials.PhoneNumber ?? "",
                    Status = userCredentials.Status,
                    CreatedDateTimeUtc = DateTime.UtcNow,
                    UpdatedDateTimeUtc = DateTime.UtcNow,
                    PlantName = userCredentials.UnitName,
                    Gender = Enum.Parse<eGender>(userCredentials.Gender.ToString()),
                    //ActualPassword = userCredentials.Password
                };
                var response = await _userManager.CreateAsync(user, userCredentials.Password);
                if (response.Succeeded)
                {
                    
                    var createdEmployee= await _userService.CreateUserEmployeeAsync(user,password,cancellationToken);
                    
                }

                if (!response.Succeeded)
                {
                    foreach (IdentityError error in response.Errors)
                    {
                        ModelState.AddModelError("errors", error.Description);
                    }
                    return new BadRequestObjectResult(ModelState);
                }
                var pathString = "./MailTemplate/Register.html";

                var builder = new StringBuilder();

                using (var reader = System.IO.File.OpenText(pathString))
                {
                    builder.Append(reader.ReadToEnd());
                }

                builder.Replace("@Name", $"{user.FirstName} {user.LastName}");
                builder.Replace("@Password", $"{userCredentials.Password}");

                builder.Replace("@UserName", user.UserName);
                await _emailSender.SendEmailAsync(user.Email, "Register", builder.ToString(),"", true);


                if (userCredentials.Roles != null)
                    foreach (var role in userCredentials.Roles)
                    {
                        var isRoleExist = await _roleManager.RoleExistsAsync(role);
                        //if (isRoleExist)//pervious code commented by raj on 22 Oct 2021, cause role was not added into AspNetRoles
                        if (isRoleExist)
                        {
                            await _userManager.AddToRoleAsync(user, role);
                        }
                    }

                JsonResult result = new JsonResult(new { Status = "ok", Message = "User Registered Successfully", result = user });
                return Ok(new { Status = "ok", Message = "User Registered Successfully", result = user });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return BadRequest();
            }
        }

        [HttpPut]
        [HttpPost]
        [Authorize]
        [Route("/api/users/update")]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await _userManager.FindByIdAsync(requestDto.Id.ToString());

                if (user == null)
                {
                    return NoContent();
                }

                user.Id = (int)requestDto.Id;
                user.Email = requestDto.Email;
                user.FirstName = requestDto.FirstName;
                user.LastName = requestDto.LastName;
                user.PhoneNumber = requestDto.PhoneNumber ?? "";
                user.Status = requestDto.Status;
                user.UpdatedDateTimeUtc = DateTime.UtcNow;

                var response = await _userManager.UpdateAsync(user);

                var existingRoles = await _userManager.GetRolesAsync(user);


                foreach (var item in existingRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, item);
                }

                if (requestDto.Roles != null)
                    foreach (var role in requestDto.Roles)
                    {
                        var isRoleExist = await _roleManager.RoleExistsAsync(role);
                        if (isRoleExist)
                        {
                            await _userManager.AddToRoleAsync(user, role);
                        }
                    }
                if (response.Succeeded)
                    return Ok();
                return BadRequest(response.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return BadRequest();
            }
        }

        /// <summary>
        /// GetUserUnitsAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("/api/users/units/{id}")]
        public async Task<IActionResult> GetUserUnitsAsync(long id)
        {
            ApiResponse<List<Unit>> apiResponse = new ApiResponse<List<Unit>>();
            List<Unit> lstUnit = new List<Unit>();
            try
            {
                var userUnits = await _userService.GetUserUnitsByIdAsync(id);
                if (userUnits == null)
                {
                    return NoContent();
                }
                else
                {
                    lstUnit = userUnits;
                }
                apiResponse = new ApiResponse<List<Unit>>() {
                    Status="ok",
                    Message = "Unit List",
                    Result = lstUnit
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<List<Unit>>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = lstUnit
                };
                return BadRequest(apiResponse);

            }
        }
    }
}   