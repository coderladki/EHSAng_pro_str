using CRM.Server.Models;
using CRM.Server.Services;
using CRM.Server.Web.Api.Controllers;
using CRM.Server.Web.Api.DataObjects;
using EHS.Server.Models.Masters;
using EHS.Server.Services.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;

namespace EHS.Server.Web.Api.Controllers
{
    public class HazardsController : BaseAuthorizeController
    {
        readonly ILogger<MasterController> _logger;
        private readonly HazardsService _hazardsService;
        private JsonSerializerSettings _serializerSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private IWebHostEnvironment hostEnvironment;


        public HazardsController(ILogger<MasterController> logger, HazardsService hazardsService, IEmailSender emailSender, IWebHostEnvironment env, UserManager<ApplicationUser> userManager)
        {
            this._logger = logger;
            _hazardsService = hazardsService;
            _userManager = userManager;
            hostEnvironment = env;
        }
        #region Hazard Master

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 09/03/2023
        /// To Add Hazard master  
        /// </summary>
        /// <param name="hazard"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/hazard/create")]
        public async Task<IActionResult> CreateUnitMaster([FromBody] Hazards hazard, CancellationToken cancellationToken)
        {
            ApiResponse<Hazards> apiResponse = null;
            Hazards hazardsMasterResponse = null;
            try
            {
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;

                var createdBy = user.Id;
                if (hazard != null)
                {
                    hazard.CreatedBy = createdBy;
                    hazard.ModifiedBy = createdBy;
                }

                hazardsMasterResponse = await _hazardsService.CreateHazardAsync(hazard, cancellationToken);
                apiResponse = new ApiResponse<Hazards>()
                {
                    Status = "ok",
                    Message = "Hazard Created Successfully",
                    Result = hazardsMasterResponse
                };
                return Ok(apiResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<Hazards>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = hazardsMasterResponse
                };
                return BadRequest(apiResponse);
            }
        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 09/03/2023
        /// To get list of hazard master  
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/hazard/getall")]
        public async Task<IActionResult> GetAllHazardsAsync()
        {
            ApiResponse<List<Hazards>> apiResponse = null;
            var responseList = new List<Hazards>();
            try
            {
                responseList = await _hazardsService.GetAllHazardsAsync(new Hazards());
                if (responseList == null)
                {
                    return NoContent();
                }

                apiResponse = new ApiResponse<List<Hazards>>()
                {
                    Status = "ok",
                    Message = "Hazard List",
                    Result = responseList
                };
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<List<Hazards>>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = responseList
                };
                return BadRequest(apiResponse);
            }

        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 09/03/2023
        /// To get  hazard master  by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/hazard/{id}")]
        public async Task<IActionResult> GetUnitByIdAsync(int id, CancellationToken cancellationToken)
        {
            ApiResponse<Hazards> apiResponse = null;
            var hazardResponse = new Hazards();
            try
            {
                Hazards hazards = new Hazards() { HazardId = id };
                var response = await _hazardsService.GetAllHazardsAsync(hazards);
                if (response == null)
                {
                    return NoContent();
                }
                else
                {
                    hazardResponse = response.FirstOrDefault();
                }

                apiResponse = new ApiResponse<Hazards>()
                {
                    Status = "ok",
                    Message = "",
                    Result = hazardResponse
                };
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<Hazards>()
                {
                    Status = "ok",
                    Message = "",
                    Result = hazardResponse
                };
                return BadRequest(apiResponse);
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 09/03/2023
        /// To Add update  unit master  
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [HttpPost]
        [Route("/api/hazard/update")]
        public async Task<IActionResult> UpdateUnitAsync([FromForm] Hazards requestDto, CancellationToken cancellationToken)
        {
            ApiResponse<Hazards> apiResponse = null;
            var hazardsResponse = new Hazards();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;
                requestDto.ModifiedBy = user.Id;
                hazardsResponse = await _hazardsService.UpdateHazardAsync(requestDto, cancellationToken);

                apiResponse = new ApiResponse<Hazards>()
                {
                    Status = "ok",
                    Message = "Hazard master updated successfully",
                    Result = hazardsResponse
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<Hazards>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = hazardsResponse
                };
                return BadRequest(apiResponse);
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 09/03/2023
        /// To delete  hazard
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("/api/hazard/delete")]
        public async Task<IActionResult> DeleteUnitAsync([FromQuery] int id)
        {
            ApiResponse<Hazards> apiResponse = null;
            var responseList = new Hazards();

            try
            {
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;
                Hazards hazards = new Hazards() { ModifiedBy = user.Id, HazardId = id };

                var resp = await _hazardsService.DeleteHazardAsync(hazards);

                apiResponse = new ApiResponse<Hazards>()
                {
                    Status = "ok",
                    Message = "Hazard deleted successfully",
                    Result = responseList,
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<Hazards>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = responseList,
                };
                return BadRequest(apiResponse);
            }
        }
        #endregion

    }
}
