using CRM.Server.Web.Api.Controllers;
using CRM.Server.Web.Api.DataObjects;
using EHS.Server.Services.Domain;
using EHS.Server.Web.Api.DataObjects.NewFolder;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using EHS.Server.Models;
using Microsoft.Extensions.Logging;
using CRM.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace EHS.Server.Web.Api.Controllers
{
    public class IncidentController : BaseAuthorizeController
    {
        private readonly IncidentService _IncidentService;
        readonly ILogger<IncidentController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public IncidentController(IncidentService incidentService, ILogger<IncidentController> logger, UserManager<ApplicationUser> userManager)
        {
            _IncidentService = incidentService;
            this._logger = logger;
            _userManager = userManager;
        }
        [HttpGet]
        [Route("/api/typeofincident/getall")]
        public async Task<IActionResult> GetAllTypeOfIncidentAsync()
        {
            ApiResponse<List<TypeOfIncident>> apiResponse = null;
            try
            {
                var TypeOfIncidentRes = await _IncidentService.GetAllTypeOfIncidentAsync();
                if (TypeOfIncidentRes == null)
                {
                    return NoContent();
                }
                apiResponse = new ApiResponse<List<TypeOfIncident>>()
                {
                    Status = "ok",
                    Message = "Type Of Incident List",
                    Result = TypeOfIncidentRes
                };
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<List<TypeOfIncident>>()
                {
                    Status = "ok",
                    Message = "Type Of Incident List",
                    Result = null
                };
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("/api/category/getall")]
        public async Task<IActionResult> GetAllCategoryAsync()
        {
            ApiResponse<List<Categories>> apiResponse = null;
            try
            {
                var categories = await _IncidentService.GetAllCategoryAsync();
                if (categories == null)
                {
                    return NoContent();
                }
                apiResponse = new ApiResponse<List<Categories>>()
                {
                    Status = "ok",
                    Message = "Category List",
                    Result = categories
                };
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<List<Categories>>()
                {
                    Status = "ok",
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(apiResponse);
            }
        }

        [HttpGet]
        [Route("/api/incidentActivity/getall")]
        public async Task<IActionResult> GetAllIncidentActivityAsync()
        {
            ApiResponse<List<IncidentActivity>> apiResponse = null;
            try
            {
                var IncidentActivity = await _IncidentService.GetAllIncidentActivityAsync();
                if (IncidentActivity == null)
                {
                    return NoContent();
                }
                apiResponse = new ApiResponse<List<IncidentActivity>>()
                {
                    Status = "ok",
                    Message = "Incident Activity List",
                    Result = IncidentActivity
                };
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<List<IncidentActivity>>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(apiResponse);
            }
        }

        [HttpGet]
        [Route("/api/bodypart/getall")]
        public async Task<IActionResult> GetAllBodyPartsAsync()
        {
            ApiResponse<List<Bodypart>> apiResponse = null;
            try
            {
                var BodyParts = await _IncidentService.GetAllBodyPartsAsync();
                if (BodyParts == null)
                {
                    return NoContent();
                }
                apiResponse = new ApiResponse<List<Bodypart>>()
                {
                    Status = "ok",
                    Message = "Body Part List",
                    Result = BodyParts
                };
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<List<Bodypart>>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(apiResponse);
            }
        }

        [HttpGet]
        [Route("/api/PPE/getall")]
        public async Task<IActionResult> GetAllPPEAsync()
        {
            ApiResponse<List<PPE>> apiResponse = null;
            try
            {
                var allppe = await _IncidentService.GetAllPPEAsync();
                if (allppe == null)
                {
                    return NoContent();
                }
                apiResponse = new ApiResponse<List<PPE>>()
                {
                    Status = "ok",
                    Message = "PPE List",
                    Result = allppe
                };
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<List<PPE>>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(apiResponse);
            }
        }
    }
}
