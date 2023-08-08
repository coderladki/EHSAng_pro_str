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
using Newtonsoft.Json;
using CRM.Server.Web.Api.DataObjects.Dashboard;

namespace CRM.Server.Web.Api.Controllers
{
    [ApiController]
    public class DashboardController : BaseAuthorizeController
    {

        readonly ILogger<DashboardController> _logger;
        private readonly DashboardService _DashboardService;
        private readonly IEmailSender _emailSender;
        private IWebHostEnvironment _env;
        private Formatting _serializerSettings;

        //public DashboardMenusController(ILogger<DashboardMenusController> logger, DashboardMenuService DashboardMenuService, IEmailSender emailSender, IWebHostEnvironment env)
        //{
        //    this._logger = logger;
        //    _DashboardMenuService = DashboardMenuService;
        //    _emailSender = emailSender;
        //    _env = env;
        //}

        public DashboardController(ILogger<DashboardController> logger, IWebHostEnvironment env, DashboardService DashboardService)
        {
            this._logger = logger;
            _DashboardService = DashboardService;
            _env = env;
        }


        [HttpGet]
        [Route("/api/DashboardMenus/getall")]
        public async Task<IActionResult> GetAllDashboardMenusAsync()
        {
            var loggedInUser = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;
            try
            {
                var dashboardMenuResponses = await _DashboardService.GetAllDashboardMenusAsync(loggedInUser.Id);
                if (dashboardMenuResponses == null)
                {
                    return NoContent();
                }
                var dashboardMenuResponseList = new List<DashboardMenuDto>();
                foreach (var dashboardMenuResponse in dashboardMenuResponses)
                {
                    dashboardMenuResponseList.Add(new DashboardMenuDto
                    {
                        Id = dashboardMenuResponse.Id,
                        Title = dashboardMenuResponse.Title,
                        Parent = dashboardMenuResponse.Parent,
                        Level = dashboardMenuResponse.Level,
                        Hierarchy = dashboardMenuResponse.Hierarchy,
                        Actualpath = dashboardMenuResponse.Actualpath
                    });

                }

                return Ok(dashboardMenuResponseList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return BadRequest();
            }
        }
    }
}
