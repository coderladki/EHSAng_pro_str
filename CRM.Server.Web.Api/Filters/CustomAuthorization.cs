using CRM.Server.Data.Features;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading;

namespace CRM.Server.Web.Api.Filters
{
    //to be used for funcation based auth
    [AttributeUsage(AttributeTargets.All)]
    public class PermissisonsAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        public List<string> permissionValue = new List<string>();
        public PermissisonsAuthorizationAttribute(params string[] permissions)
        {
            if (permissions != null)
            {
                foreach (var permission in permissions)
                {
                    permissionValue.Add(permission);
                }
            }
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            try
            {                
                foreach (var role in permissionValue)
                {
                    var pp = filterContext.HttpContext.User.Claims.FirstOrDefault(u => u.Type == "Permission" && u.Value ==  role);
                   if (filterContext.HttpContext.User.Claims.FirstOrDefault(u => u.Type == "Permission" && u.Value == role) == null)
                    {
                        filterContext.Result = new JsonResult("Please Provide" + role)
                        {
                            Value = new
                            {
                                Status = "Error",
                                Message = "Please provide required permission."
                            },
                        };
                        break;
                    }
                }
                return;
            }
            catch(Exception ex)
            {
                filterContext.Result = new JsonResult("Please provide required permission.")
                {
                    Value = new
                    {
                        Status = "Error",
                        Message = "Please provide required permisiion."
                    },
                };
            }
        }
    }

    //to be used for controller based auth : sagar
    // We might not need any such implementation as we are focussing on permissions provided by Roles. Even individual permissions can be set: vaibhav
    [AttributeUsage(AttributeTargets.Class)]
    public class CRMRoleAuth : Attribute, IAuthorizationFilter
    {
        public List<string> permissionValue = null;
        public CRMRoleAuth(params string[] permissions)
        {
            if (permissions != null)
            {
                foreach (var permission in permissions)
                {
                    permissionValue.Add(permission);
                }
            }
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            try
            {
                var aspUserRole = GlobalPreferences.AspNetUserRoles;
                foreach (var role in permissionValue)
                {
                    if (!aspUserRole.Contains(role))
                    {
                        filterContext.Result = new JsonResult("Please Provide" + role)
                        {
                            Value = new
                            {
                                Status = "Error",
                                Message = "Please provide required role."
                            },
                        };
                        break;
                    }
                }
                return;
            }
            catch (Exception ex)
            {
                filterContext.Result = new JsonResult("Please provide required role.")
                {
                    Value = new
                    {
                        Status = "Error",
                        Message = "Please provide required role."
                    },
                };
            }
        }
    }
}
