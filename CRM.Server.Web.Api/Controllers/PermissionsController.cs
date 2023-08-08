using CRM.Server.Models;
using CRM.Server.Services.Domain;
using CRM.Server.Web.Api.DataObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AuthorizeAttribute = CRM.Server.Web.Api.Filters.AuthorizeAttribute;

namespace CRM.Server.Web.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class PermissionsController : Controller
    {
        private readonly PermissionsService _PermissionService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserService _userSerivce;

        public PermissionsController(PermissionsService permissionsService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,UserService userService)
        {
            _PermissionService = permissionsService;
            _userManager = userManager;
            _roleManager = roleManager;
            _userSerivce = userService;
        }

        [HttpPost("api/Permissions/Merge")]
        public async Task<IActionResult> MergePermission(Permission permission, CancellationToken cancellationToken)
        {
            await _PermissionService.MergePermission(permission, cancellationToken).ConfigureAwait(false);
            return Ok(new { status = "ok", Result = permission, Message = "success" });
        }
        [HttpGet("api/Permissions/GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _PermissionService.GetAll(cancellationToken).ConfigureAwait(false);
            var PermissionList = new List<string>();
            return Ok(new { status = "ok", message = "Permission List", Result = result });
        }

        [HttpGet("api/Permissions/GetAllRolePermissions/{RoleId}")]
        public async Task<IActionResult> GetAllRolePermissions(int RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId.ToString());
            if (role == null)
            {
                var ErrorMessage = $"Role with Id = {RoleId} cannot be found";
                var apiErrorResponse = new ApiResponse<string>("error", ErrorMessage, "");
                return new BadRequestObjectResult(apiErrorResponse);
            }
            var ExistingClaims = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);
            var list = ExistingClaims.Where(c => c.Type == "Permission");
            var AssignedRolePermissions = new List<string>();
            list.ToList().ForEach(claim => { AssignedRolePermissions.Add(claim.Value); });
            var apiResponse = new ApiResponse<List<string>>("ok", "All Permissions", AssignedRolePermissions);
            return Ok(apiResponse);
        }

        [HttpGet("api/Permissions/GetAllUserPermissions/{UserId}")]
        public async Task<IActionResult> GetAllUserPermissions(int UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            if (user == null)
            {
                var ErrorMessage = $"user with Id = {UserId} cannot be found";
                var apiErrorResponse = new ApiResponse<string>("error", ErrorMessage, "");
                return new BadRequestObjectResult(apiErrorResponse);
            }
            var ExistingClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
            var list = ExistingClaims.Where(c => c.Type == "Permission");
            var AssignedUserPermissions = new List<string>();
            list.ToList().ForEach(claim => { AssignedUserPermissions.Add(claim.Value); });
            var apiResponse = new ApiResponse<List<string>>("ok", "All User Permissions", AssignedUserPermissions);
            return Ok(apiResponse);
        }


        [HttpGet("api/Permissions/GetAllUserNavigationPermissions/{UserId}")]
        public async Task<IActionResult> GetAllUserNavigationPermissions(int UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            if (user == null)
            {
                var ErrorMessage = $"user with Id = {UserId} cannot be found";
                var apiErrorResponse = new ApiResponse<string>("error", ErrorMessage, "");
                return new BadRequestObjectResult(apiErrorResponse);
            }
            var ExistingClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
            var list = ExistingClaims.Where(c => c.Type == "Navigation Permission");
            var AssignedUserPermissions = new List<string>();
            list.ToList().ForEach(claim => { AssignedUserPermissions.Add(claim.Value); });
            var apiResponse = new ApiResponse<List<string>>("ok", "All User Navigation Permissions", AssignedUserPermissions);
            return Ok(apiResponse);
        }
        [HttpPost("api/Permissions/AddPermissions/Role/{RoleId}")]
        public async Task<IActionResult> AddRoleClaims(CancellationToken cancellationToken,[FromRoute] int RoleId, [FromBody] object obj)
        {
            var PermissionsToAdd = JsonConvert.DeserializeObject<List<string>>(obj.ToString());
            var role = await _roleManager.FindByIdAsync(RoleId.ToString());
            if (role == null)
            {
                var ErrorMessage = $"Role with Id = {RoleId} cannot be found";
                var apiResponse = new ApiResponse<string>("error",ErrorMessage,"" );
                return new BadRequestObjectResult(apiResponse);
            }
            
            var AvailablePermissions = await _PermissionService.GetAll(cancellationToken).ConfigureAwait(false);
            var RolePermissions = new List<string>();
            var ExistingClaims = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);
            var UsersInRole = await _userSerivce.GetAllUserByRoleIdAsync(RoleId).ConfigureAwait(false);
            foreach (var claim in ExistingClaims.Where(a => a.Type == "Permission"))
            {
                if(PermissionsToAdd.FirstOrDefault(p => p == claim.Value)== null)
                {
                    await _roleManager.RemoveClaimAsync(role, claim).ConfigureAwait(false);
                    
                    if (UsersInRole != null)
                    {
                        foreach(var user in UsersInRole)
                        {
                            await _userManager.RemoveClaimAsync(user,claim).ConfigureAwait(false);
                        }                        
                    }
                }
            }
            foreach (var p in PermissionsToAdd)
            {
                var permission = AvailablePermissions.FirstOrDefault(ap => ap.Name == p && ap.Status);
                if (permission != null)
                {
                    Claim claim = new Claim("Permission", p);
                    if (ExistingClaims.FirstOrDefault(c=>c.Type =="Permission" && c.Value == p)== null)
                    {
                       await _roleManager.AddClaimAsync(role, claim).ConfigureAwait(false);
                        if (UsersInRole != null)
                        {
                            foreach (var user in UsersInRole)
                            {
                                await _userManager.AddClaimAsync(user, claim).ConfigureAwait(false);
                            }
                        }
                    }                    
                    RolePermissions.Add(p);
                }
            }
            return Ok(new { status = "ok", Result = new { Rolepermissions = RolePermissions }, Message = "Permissions to Role Updated Successfully!" });            
        }
        
        [HttpPost("api/Permissions/Addpermissions/user/{UserId}")]
        public async Task<IActionResult> UpdateUserPermissions(CancellationToken cancellationToken, [FromRoute] int UserId, [FromBody] object obj)
        {
            var PermissionsToAdd = JsonConvert.DeserializeObject<List<string>>(obj.ToString());
            var user = await _userManager.FindByIdAsync(UserId.ToString()).ConfigureAwait(false);
            if (user == null)
            {
                var ErrorMessage = $"user with Id = {UserId} cannot be found";
                var apiResponse = new { status = "error", message = ErrorMessage, result = "" };
                return new BadRequestObjectResult(apiResponse);
            }
            else
            {
                var UserPermissions = new List<Permission>();
                var AvailablePermissions = await _PermissionService.GetAll(cancellationToken).ConfigureAwait(false);
                var ExistingClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
                var claimsToBeRemoved = new List<Claim>();
                foreach (var claim in ExistingClaims.Where(a => a.Type == "Permission"))
                {
                    if (PermissionsToAdd.FirstOrDefault(p => p == claim.Value ) == null)
                    {
                        claimsToBeRemoved.Add(claim);                        
                    }                    
                }
                if (claimsToBeRemoved.Count > 0)
                {
                    await _userManager.RemoveClaimsAsync(user, claimsToBeRemoved).ConfigureAwait(false);
                }
                
                var ExistingActivePermissionsToAdd = new List<Permission>();
                foreach (var p in PermissionsToAdd)
                {
                    var ExistingActivePermission = AvailablePermissions.FirstOrDefault(ap => ap.Name == p && ap.Status == true);
                    ExistingActivePermissionsToAdd.Add(ExistingActivePermission);
                }
                if (ExistingActivePermissionsToAdd.Count > 0)
                {
                    var claimsToAdd = new List<Claim>();
                    ExistingActivePermissionsToAdd.ForEach(p =>
                    {
                        claimsToAdd.Add(new Claim("Permission", p.Name));
                    });
                    IdentityResult result = await _userManager.AddClaimsAsync(user, claimsToAdd).ConfigureAwait(false);
                    if (result.Succeeded)
                    {
                        UserPermissions = ExistingActivePermissionsToAdd;
                    }
                    return Ok(new { status = "ok", result = new { UserPermissions = UserPermissions }, message = "Permissions to User Updated Successfully!" });
                }
                else
                {
                    var ErrorMessage = $"Either the permissiosns do not exist or they are not active.";
                    var apiResponse = new { status = "error", message = ErrorMessage, result = "" };
                    return new BadRequestObjectResult(apiResponse);
                }
                
            }            
        }

        [HttpPost("api/Permissions/AddNavigationPermissions/user/{UserId}")]
        public async Task<IActionResult> UpdateUserNavigationPermissions(CancellationToken cancellationToken, [FromRoute] int UserId, [FromBody] object obj)
        {
            var PermissionsToAdd = JsonConvert.DeserializeObject<List<string>>(obj.ToString());
            var user = await _userManager.FindByIdAsync(UserId.ToString()).ConfigureAwait(false);
            if (user == null)
            {
                var ErrorMessage = $"user with Id = {UserId} cannot be found";
                var apiResponse = new { status = "error", message = ErrorMessage, result = "" };
                return new BadRequestObjectResult(apiResponse);
            }
            else
            {
                var UserPermissions = new List<Permission>();
                var AvailablePermissions = await _PermissionService.GetAllNavigation(cancellationToken).ConfigureAwait(false);
                var ExistingClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
                var claimsToBeRemoved = new List<Claim>();
                foreach (var claim in ExistingClaims.Where(a => a.Type == "Navigation Permission"))
                {
                    if (PermissionsToAdd.FirstOrDefault(p => p == claim.Value) == null)
                    {
                        claimsToBeRemoved.Add(claim);
                    }
                }
                if (claimsToBeRemoved.Count > 0)
                {
                    await _userManager.RemoveClaimsAsync(user, claimsToBeRemoved).ConfigureAwait(false);
                }

                var ExistingActivePermissionsToAdd = new List<Permission>();
                foreach (var p in PermissionsToAdd)
                {
                    var ExistingActivePermission = AvailablePermissions.FirstOrDefault(ap => ap.Name == p && ap.Status == true);
                    ExistingActivePermissionsToAdd.Add(ExistingActivePermission);
                }
                if (ExistingActivePermissionsToAdd.Count > 0)
                {
                    var claimsToAdd = new List<Claim>();
                    ExistingActivePermissionsToAdd.ForEach(p =>
                    {
                        claimsToAdd.Add(new Claim("Navigation Permission", p.Name));
                    });
                    IdentityResult result = await _userManager.AddClaimsAsync(user, claimsToAdd).ConfigureAwait(false);
                    if (result.Succeeded)
                    {
                        UserPermissions = ExistingActivePermissionsToAdd;
                    }
                    return Ok(new { status = "ok", result = new { UserPermissions = UserPermissions }, message = "Navigation Permissions to User Updated Successfully!" });
                }
                else
                {
                    var ErrorMessage = $"Either the permissiosns do not exist or they are not active.";
                    var apiResponse = new { status = "error", message = ErrorMessage, result = "" };
                    return new BadRequestObjectResult(apiResponse);
                }

            }
        }

        [HttpGet("api/Permissions/GetAllNavigationPermissions/{RoleId}")]
        public async Task<IActionResult> GetAllNavigationPermissions(int RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId.ToString());
            if (role == null)
            {
                var ErrorMessage = $"Role with Id = {RoleId} cannot be found";
                var apiErrorResponse = new ApiResponse<string>("error", ErrorMessage, "");
                return new BadRequestObjectResult(apiErrorResponse);
            }
            var ExistingClaims = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);
            var list = ExistingClaims.Where(c => c.Type == "Navigation Permission");
            var AssignedRolePermissions = new List<string>();
            list.ToList().ForEach(claim => { AssignedRolePermissions.Add(claim.Value); });
            var apiResponse = new ApiResponse<List<string>>("ok", "All Navigation Permissions", AssignedRolePermissions);
            return Ok(apiResponse);
        }

        [HttpPost("api/Permissions/AddNavigationPermissions/Role/{RoleId}")]
        public async Task<IActionResult> AddNavigationPermissions(CancellationToken cancellationToken, [FromRoute] int RoleId, [FromBody] object obj)
        {
            var PermissionsToAdd = JsonConvert.DeserializeObject<List<string>>(obj.ToString());
            var role = await _roleManager.FindByIdAsync(RoleId.ToString());
            if (role == null)
            {
                var ErrorMessage = $"Role with Id = {RoleId} cannot be found";
                var apiResponse = new ApiResponse<string>("error", ErrorMessage, "");
                return new BadRequestObjectResult(apiResponse);
            }

            var AvailablePermissions = await _PermissionService.GetAllNavigation(cancellationToken).ConfigureAwait(false);
            var RolePermissions = new List<string>();
            var ExistingClaims = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);
            var UsersInRole = await _userSerivce.GetAllUserByRoleIdAsync(RoleId).ConfigureAwait(false);
            foreach (var claim in ExistingClaims.Where(a=>a.Type == "Navigation Permission"))
            {
                if (PermissionsToAdd.FirstOrDefault(p => p == claim.Value) == null)
                {
                    await _roleManager.RemoveClaimAsync(role, claim).ConfigureAwait(false);

                    if (UsersInRole != null)
                    {
                        foreach (var user in UsersInRole)
                        {
                            await _userManager.RemoveClaimAsync(user, claim).ConfigureAwait(false);
                        }
                    }
                }
            }
            foreach (var p in PermissionsToAdd)
            {
                var permission = AvailablePermissions.FirstOrDefault(ap => ap.Name == p && ap.Status);
                if (permission != null)
                {
                    Claim claim = new Claim("Navigation Permission", p);
                    if (ExistingClaims.FirstOrDefault(c => c.Type == "Navigation Permission" && c.Value == p) == null)
                    {
                        await _roleManager.AddClaimAsync(role, claim).ConfigureAwait(false);
                        if (UsersInRole != null)
                        {
                            foreach (var user in UsersInRole)
                            {
                                await _userManager.AddClaimAsync(user, claim).ConfigureAwait(false);
                            }
                        }
                    }
                    RolePermissions.Add(p);
                }
            }
            return Ok(new { status = "ok", Result = new { Rolepermissions = RolePermissions }, Message = "Permissions to Role Updated Successfully!" });
        }

        [HttpGet("api/Permissions/GetAllNavigation")]
        public async Task<IActionResult> GetAllNavigation(CancellationToken cancellationToken)
        {
            var result = await _PermissionService.GetAllNavigation(cancellationToken).ConfigureAwait(false);
            var PermissionList = new List<string>();
            return Ok(new { status = "ok", message = "Navigation List", Result = result });
        }

    }
}
