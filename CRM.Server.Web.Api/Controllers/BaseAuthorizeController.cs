using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using AuthorizeAttribute = CRM.Server.Web.Api.Filters.AuthorizeAttribute;

namespace CRM.Server.Web.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class BaseAuthorizeController : ControllerBase
    {
        protected string UserName
        {
            get
            {
                if(HttpContext.Items["CurrentLoggedUser"].ToString() == ClaimTypes.Email)
                {
                    return HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                }
                return null;
            }
        } 
    }
}
