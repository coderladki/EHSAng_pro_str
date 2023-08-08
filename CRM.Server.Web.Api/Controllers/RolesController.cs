using CRM.Server.Models;
using CRM.Server.Services;
using CRM.Server.Services.Domain;
using CRM.Server.Web.Api.DataObjects;
using CRM.Server.Web.Api.User.DataObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CRM.Server.Web.Api.Controllers
{
    [Route("api/roles")]
    //[Authorize(Roles = "Admin, SuperAdmin")]
    [ApiController]
    public class RolesController : BaseAuthorizeController
    {
        readonly ILogger<UsersController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserService _userService;
        private readonly IEmailSender _emailSender;
        private IWebHostEnvironment _env;


        public RolesController(ILogger<UsersController> logger, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, UserService userService, IEmailSender emailSender, IWebHostEnvironment env)
        {
            this._logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
            _emailSender = emailSender;
            _env = env;

        }

        [HttpGet("list")]

        public IActionResult ListRole()
        {
            //var support =  _roleManager.SupportsQueryableRoles;
            // _roleManager.FindByIdAsync(1);
            var roles = _roleManager.Roles;
            //return BadRequest("test");

            return Ok(roles);

        }
        [HttpGet("roleRights")]

        public IActionResult ListRoleRights()
        {
            //var support =  _roleManager.SupportsQueryableRoles;
            // _roleManager.FindByIdAsync(1);
            // var roles = _roleManager.Roles;
            //return BadRequest("test");

            return Ok();

        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRole(CreateRoleDto dto)
        {
            if (ModelState.IsValid)
            {
                // We just need to specify a unique role name to create a new role
                ApplicationRole applicationRole = new ApplicationRole
                {
                    Name = dto.RoleName,
                    CreatedDateTimeUtc = System.DateTime.Now,
                    UpdatedDateTimeUtc = System.DateTime.Now,
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };

                // Saves the role in the underlying AspNetRoles table
                IdentityResult result = await _roleManager.CreateAsync(applicationRole);

                if (result.Succeeded)
                {
                    // return RedirectToAction("getall", "RolesController");
                    return Ok(result);
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("errors", error.Description);
                }
                return new BadRequestObjectResult(ModelState);
                //return Ok(result.Errors);

            }
            else
            {
                return new BadRequestObjectResult(ModelState);
            }
        }

        [HttpGet("{roleid}")]

        public async Task<IActionResult> getRole([FromRoute] int roleid)
        {
            //we need to check roleid is passed in query and is a valid roleid   
            if (roleid != 0)
            {
                //getting role with given roleid from underlying AspNetRoles table
                // Find the role by Role ID
                var role = await _roleManager.FindByIdAsync(roleid.ToString());
                if (role == null)
                {
                    var ErrorMessage = $"Role with Id = {roleid} cannot be found";
                    return new BadRequestObjectResult(ErrorMessage);
                }
                else
                {
                    var model = new ViewRoleDto
                    {
                        Id = role.Id,
                        RoleName = role.Name
                    };
                    var ApplicationUsers = await _userManager.GetUsersInRoleAsync(role.Name);
                    var users = new List<string>();
                    foreach (var u in ApplicationUsers)
                    {
                        users.Add(u.UserName);
                    }
                    //foreach (var user in _userManager.Users)
                    //{
                    //    if (await _userManager.IsInRoleAsync(user, role.Name))
                    //    {
                    //        users.Add(user.UserName);
                    //    }
                    //}
                    model.Users = users;
                    return Ok(model);
                }
            }
            else
            {
                return new BadRequestObjectResult("Please give valid Roleid");
            }
        }
        [HttpPost("edit")]

        public async Task<IActionResult> EditRole([FromBody] EditRoleDto dto)
        {
            //we need to check roleid is passed in query and is a valid roleid   
            if (dto.Id != 0)
            {
                //getting role with given roleid from underlying AspNetRoles table
                // Find the role by Role ID
                var role = await _roleManager.FindByIdAsync(dto.Id.ToString());
                if (role == null)
                {
                    var ErrorMessage = $"Role with Id = {dto.Id} cannot be found";
                    return new BadRequestObjectResult(ErrorMessage);
                }
                else
                {
                    role.Name = dto.RoleName;
                    role.NormalizedName = dto.RoleName.ToUpper();
                    role.UpdatedDateTimeUtc = DateTime.UtcNow;
                    role.ConcurrencyStamp = Guid.NewGuid().ToString();
                    var result = await _roleManager.UpdateAsync(role);
                    return Ok(result);
                }
            }
            else
            {
                return new BadRequestObjectResult("Please give valid Roleid");
            }
        }

        [HttpGet("getUsersInRole/{roleId}")]

        public async Task<IActionResult> GetUsersInRole([FromRoute] int roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                var ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return new BadRequestObjectResult(ErrorMessage);
            }
            else
            {
                var model = new List<UserRoleDto>();

                var users = new List<string>();
               foreach (var user in _userManager.Users)
                {
                    var u = new UserRoleDto()
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        FirstName= user.FirstName,
                        LastName= user.LastName

                    };

                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        u.IsSelected = true;
                    }
                    else
                    {
                        u.IsSelected = false;
                    }
                    model.Add(u);
                }
                return Ok(model);
            }
        }
        [HttpPost("editusersInRole")]

        public async Task<IActionResult> EditUsersInRole([FromBody] UpdateUsersInRoleDto updateUsersInRoleDto)
        {
            var role = await _roleManager.FindByIdAsync(updateUsersInRoleDto.RoleId.ToString());
            if (role == null)
            {
                var ErrorMessage = $"Role with Id = {updateUsersInRoleDto.RoleId} cannot be found";
                return new BadRequestObjectResult(ErrorMessage);
            }
            else
            {
                foreach (var u in updateUsersInRoleDto.userRoleModelList)
                {
                    var user = await _userManager.FindByIdAsync(u.UserId.ToString());
                    IdentityResult result = null;
                    var IsInRoleAsync = await _userManager.IsInRoleAsync(user, role.Name);
                    if (u.IsSelected && !IsInRoleAsync)
                    {
                        result = await _userManager.AddToRoleAsync(user, role.Name);
                    }
                    else if (!u.IsSelected && IsInRoleAsync)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        continue;
                    }
                }

                return Ok();

            }
        }

    }           
}
