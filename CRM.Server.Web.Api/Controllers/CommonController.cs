using CRM.Server.Models.Common;
using CRM.Server.Services;
using CRM.Server.Services.Domain;
using CRM.Server.Web.Api.DataObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Server.Web.Api.Controllers
{
    public class CommonController : BaseAuthorizeController
    {
        readonly ILogger<CommonController> _logger;
        private readonly CommonService _commonService;
        private readonly IEmailSender _emailSender;
        private IWebHostEnvironment _env;
        private JsonSerializerSettings _serializerSettings;

        public CommonController(ILogger<CommonController> logger, CommonService commonService, IEmailSender emailSender, IWebHostEnvironment env)
        {
            this._logger = logger;
            _commonService = commonService;
            _emailSender = emailSender;
            _env = env;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        [HttpGet]
        // [Authorize(AuthenticationSchemes = "Bearer")]
        
        [Route("/api/getAllState")]
        public async Task<IActionResult> GetAllStateAsync()
        {
            try
            {
                var stateResponses = await _commonService.GetAllStateAsync();
                if (stateResponses == null)
                {
                    return NoContent();
                }
                var stateResponseList = new List<State_Master>();
                foreach (var stateResponse in stateResponses)
                {
                    stateResponseList.Add(new State_Master
                    {
                        StateCode = stateResponse.StateCode,
                        StateName = stateResponse.StateName,
                        InputStateCode = stateResponse.InputStateCode,
                        STATUS = stateResponse.STATUS,
                        STAMP = stateResponse.STAMP,
                        CreateBy = stateResponse.CreateBy,
                        UpdatedBy = stateResponse.UpdatedBy
                    });
                }
                ApiResponse<List<State_Master>> apiResponse = new ApiResponse<List<State_Master>>()
                {
                    Status = "ok",
                    Message = "State List",
                    Result = stateResponseList
                };
                var json = JsonConvert.SerializeObject(apiResponse, _serializerSettings);
                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return BadRequest(new { Status = "error", Message = ex.Message });
            }
        }


        [HttpGet]
        // [Authorize(AuthenticationSchemes = "Bearer")]
        
        [Route("/api/getAllDistricts")]
        public async Task<IActionResult> GetAllDistrictsAsync([FromQuery] string StateName)
        {
            try
            {
                var districtResponses = await _commonService.GetAllDistrictsAsync(StateName);
                if (districtResponses == null)
                {
                    return NoContent();
                }
                var districtResponsesList = new List<Districts>();
                foreach (var districtResponse in districtResponses)
                {
                    districtResponsesList.Add(new Districts
                    {
                        State = districtResponse.State,
                        District = districtResponse.District,
                        StateType = districtResponse.StateType,
                    });
                }
                ApiResponse<List<Districts>> apiResponse = new ApiResponse<List<Districts>>()
                {
                    Status = "ok",
                    Message = "District List",
                    Result = districtResponsesList
                };
                var json = JsonConvert.SerializeObject(apiResponse, _serializerSettings);
                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return BadRequest(new { Status = "error", Message = ex.Message });
            }
        }
    }
}