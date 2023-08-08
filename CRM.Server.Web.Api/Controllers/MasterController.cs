using CRM.Server.Services;
using CRM.Server.Web.Api.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System.Threading.Tasks;
using System.Threading;
using System;
using EHS.Server.Web.Api.DataObjects.NewFolder;
using EHS.Server.Models.Masters;
using EHS.Server.Services.Domain;
using CRM.Server.Web.Api.DataObjects;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using CRM.Server.Services.Domain;
using CRM.Server.Web.Api.User.DataObjects;
using CRM.Server.Models;
using CorePush.Apple;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Http;
using ExcelDataReader;
using Microsoft.Extensions.Hosting.Internal;
using NPOI.Util;
using MoreLinq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using DocumentFormat.OpenXml.Spreadsheet;
using NPOI.HPSF;
using NPOI.OpenXmlFormats.Dml.Chart;
using static ClosedXML.Excel.XLPredefinedFormat;
using DateTime = System.DateTime;
using DocumentFormat.OpenXml.Bibliography;
using ClosedXML;
using Department = EHS.Server.Models.Masters.Department;
using DocumentFormat.OpenXml.Office2013.Word;

namespace EHS.Server.Web.Api.Controllers
{
    public class MasterController : BaseAuthorizeController
    {
        readonly ILogger<MasterController> _logger;
        private readonly MasterService _masterService;
        private JsonSerializerSettings _serializerSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private IWebHostEnvironment hostEnvironment;


        public MasterController(ILogger<MasterController> logger, MasterService masterService, IEmailSender emailSender, IWebHostEnvironment env, UserManager<ApplicationUser> userManager)
        {
            this._logger = logger;
            _masterService = masterService;
            _userManager = userManager;
            hostEnvironment = env;
        }


        #region Division Master


        [HttpPost]
        [Route("/api/division/create")]
        public async Task<IActionResult> Create([FromBody] DivisionDto divisionDto, CancellationToken cancellationToken)
        {
            try
            {

                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;

                var createdBy = user.FirstName;

                var division = new Division
                {
                    Name = divisionDto.Name,
                    CreatedBy = createdBy,
                    CreatedDateTimeUtc = DateTime.Now,
                    UpdatedBy = divisionDto.UpdatedBy,
                    UpdatedDateTimeUtc = DateTime.Now,
                };

                await _masterService.CreateDivisionAsync(division, cancellationToken);
                return Ok(new
                {
                    status = "ok",
                    message = "Division Created Successfully",
                    result = ""
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return BadRequest();
            }
        }

        [HttpGet]
        // [Authorize(AuthenticationSchemes = "Bearer")]

        [Route("/api/division/getall")]
        public async Task<IActionResult> GetAllDivisionAsync()
        {
            try
            {
                var divisionResponses = await _masterService.GetAllDivisionAsync();
                if (divisionResponses == null)
                {
                    return NoContent();
                }
                var divisionResponseList = new List<DivisionDto>();
                foreach (var divisionResponse in divisionResponses)
                {
                    divisionResponseList.Add(new DivisionDto
                    {
                        Id = divisionResponse.Id,
                        Name = divisionResponse.Name,
                        CreatedBy = divisionResponse.CreatedBy,
                        CreatedDateTimeUtc = divisionResponse.CreatedDateTimeUtc,
                        UpdatedBy = divisionResponse.UpdatedBy,
                        UpdatedDateTimeUtc = divisionResponse.UpdatedDateTimeUtc
                    });

                }
                ApiResponse<List<DivisionDto>> apiResponse = new ApiResponse<List<DivisionDto>>()
                {
                    Status = "ok",
                    Message = "Division List",
                    Result = divisionResponseList
                };
                var json = JsonConvert.SerializeObject(apiResponse, _serializerSettings);
                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return BadRequest();
            }
        }

        [HttpGet]
        // [Authorize(AuthenticationSchemes = "Bearer")]//, Roles = "Admin")]

        [Route("/api/division/{id}")]
        public async Task<IActionResult> GetDivisionAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                var divisionResponse = await _masterService.FindDivisionByIdAsync(id.ToString(), cancellationToken);
                if (divisionResponse == null)
                {
                    return NoContent();
                }

                var division = new Division
                {
                    Id = divisionResponse.Id,
                    Name = divisionResponse.Name
                };
                ApiResponse<Division> apiResponse = new ApiResponse<Division>()
                {
                    Status = "ok",
                    Message = "",
                    Result = division
                };
                var json = JsonConvert.SerializeObject(apiResponse, _serializerSettings);
                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return BadRequest();
            }
        }

        [HttpPut]
        [HttpPost]
        // // [Authorize(AuthenticationSchemes = "Bearer")]

        [Route("/api/division/update")]
        public async Task<IActionResult> UpdateDivisionAsync(DivisionDto requestDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var divisionResponse = await _masterService.FindDivisionByIdAsync(requestDto.Id.ToString(), cancellationToken);

                if (divisionResponse == null)
                {
                    return NoContent();
                }

                divisionResponse.Id = (int)requestDto.Id;
                divisionResponse.Name = requestDto.Name;

                var response = await _masterService.UpdateDivisionAsync(divisionResponse, cancellationToken);
                ApiResponse<string> apiResponse = new ApiResponse<string>()
                {
                    Status = "ok",
                    Message = "Division Updated Successfully",
                    Result = ""
                };

                var json = JsonConvert.SerializeObject(apiResponse, _serializerSettings);
                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return BadRequest();
            }
        }

        [HttpPost]
        //// [Authorize(AuthenticationSchemes = "Bearer")]

        [Route("/api/division/delete")]
        public async Task<IActionResult> DeleteDivisionAsync([FromQuery] int id)
        {

            try
            {
                var productResponse = await _masterService.DeleteDivisionAsync(id);

                ApiResponse<string> apiResponse = new ApiResponse<string>()
                {
                    Status = "ok",
                    Message = "Division Delete Successfully",
                    Result = ""
                };

                var json = JsonConvert.SerializeObject(apiResponse, _serializerSettings);
                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return BadRequest();
            }
        }

        #endregion

        #region Email Master
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 28/02/2023
        /// To Add email master  
        /// </summary>
        /// <param name="emailsMaster"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/email/create")]
        public async Task<IActionResult> CreateEmailMaster([FromBody] EmailsMaster emailsMaster, CancellationToken cancellationToken)
        {
            ApiResponse<EmailsMaster> apiResponse = null;
            EmailsMaster emailMasterResponse = null;
            try
            {
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;

                var createdBy = user.Id;
                if (emailsMaster != null)
                {
                    emailsMaster.CreatedBy = createdBy;
                    emailsMaster.ModifiedBy = createdBy;
                }

                emailMasterResponse = await _masterService.AddUpdateEmailMasterAsync(emailsMaster, cancellationToken);
                apiResponse = new ApiResponse<EmailsMaster>()
                {
                    Status = "ok",
                    Message = "Email Master Created Successfully",
                    Result = emailMasterResponse
                };
                return Ok(apiResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<EmailsMaster>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = emailMasterResponse
                };
                return BadRequest(apiResponse);
            }
        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 28/02/2023
        /// To get list of email master  
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/email/getall")]
        public async Task<IActionResult> GetAllEmailAsync()
        {
            ApiResponse<List<EmailsMaster>> apiResponse = null;
            var emailResponseList = new List<EmailsMaster>();
            try
            {
                var emailMasterResponses = await _masterService.GetAllEmailMasterAsync(new EmailsMaster());
                if (emailMasterResponses == null)
                {
                    return NoContent();
                }

                apiResponse = new ApiResponse<List<EmailsMaster>>()
                {
                    Status = "ok",
                    Message = "Email List",
                    Result = emailMasterResponses
                };
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<List<EmailsMaster>>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = emailResponseList
                };
                return BadRequest(apiResponse);
            }

        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 28/02/2023
        /// To get  email master  by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/email/{id}")]
        public async Task<IActionResult> GetEmailMasterByIdAsync(long id, CancellationToken cancellationToken)
        {
            ApiResponse<EmailsMaster> apiResponse = null;
            var emailResponse = new EmailsMaster();
            try
            {
                EmailsMaster emailsMaster = new EmailsMaster() { EmailMasterId = id };
                var response = await _masterService.GetAllEmailMasterAsync(emailsMaster);
                if (response == null)
                {
                    return NoContent();
                }
                else
                {
                    emailResponse = response.FirstOrDefault();
                }

                apiResponse = new ApiResponse<EmailsMaster>()
                {
                    Status = "ok",
                    Message = "",
                    Result = emailResponse
                };
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<EmailsMaster>()
                {
                    Status = "ok",
                    Message = "",
                    Result = emailResponse
                };
                return BadRequest(apiResponse);
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 28/02/2023
        /// To Add update  email master  
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [HttpPost]
        [Route("/api/email/update")]
        public async Task<IActionResult> UpdateEmailMasterAsync(EmailsMaster requestDto, CancellationToken cancellationToken)
        {
            ApiResponse<EmailsMaster> apiResponse = null;
            var emailResponse = new EmailsMaster();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;
                requestDto.ModifiedBy = user.Id;
                emailResponse = await _masterService.AddUpdateEmailMasterAsync(requestDto, cancellationToken);

                apiResponse = new ApiResponse<EmailsMaster>()
                {
                    Status = "ok",
                    Message = "Email master Updated Successfully",
                    Result = emailResponse
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<EmailsMaster>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = emailResponse
                };
                return BadRequest(apiResponse);
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 28/02/2023
        /// To delete  email master 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("/api/email/delete")]
        public async Task<IActionResult> DeleteEmailMasterAsync([FromQuery] int id)
        {
            ApiResponse<EmailsMaster> apiResponse = null;
            var emailResponseList = new EmailsMaster();

            try
            {
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;
                EmailsMaster emailsMaster = new EmailsMaster() { ModifiedBy = user.Id, EmailMasterId = id };

                emailResponseList = await _masterService.DeleteEmailMasterAsync(emailsMaster);

                apiResponse = new ApiResponse<EmailsMaster>()
                {
                    Status = "ok",
                    Message = "Email master delete Successfully",
                    Result = emailResponseList,
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<EmailsMaster>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = emailResponseList,
                };
                return BadRequest(apiResponse);
            }
        }
        #endregion


        #region Employee Master

        [HttpPost]
        [Route("/api/employee/create")]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDto employeeDto, CancellationToken cancellationToken)
        {
            try
            {

                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;

                var createdBy = user.Id;

                var employee = new Employee
                {
                    DateofBirth = employeeDto.DateofBirth,
                    FirstName = employeeDto.FirstName,
                    LastName = employeeDto.LastName,
                    Department = employeeDto.Department,
                    Designation = employeeDto.Designation,
                    Unit = employeeDto.Unit,
                    Mobile = employeeDto.Mobile,
                    Email = employeeDto.Email,
                    Password = employeeDto.Password,
                    Other = employeeDto.Other,
                    Agency = employeeDto.Agency,
                    XEmployee = employeeDto.XEmployee,
                    Gender = employeeDto.Gender,
                    CreatedBy = createdBy.ToString(),
                    UserId = createdBy,
                    CreatedDateTimeUtc = DateTime.Now,
                    UpdatedBy = employeeDto.UpdatedBy,
                    UpdatedDateTimeUtc = DateTime.Now,
                };

                await _masterService.CreateEmployeeAsync(employee, _userManager, cancellationToken);
                return Ok(new
                {
                    status = "ok",
                    message = "Employee Created Successfully",
                    result = ""
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return BadRequest();
            }
        }



        [HttpGet]
        // [Authorize(AuthenticationSchemes = "Bearer")]

        [Route("/api/employee/getall")]
        public async Task<IActionResult> GetAllEmployeeAsync()
        {
            try
            {
                var employeeResponses = await _masterService.GetAllEmployeeAsync();
                if (employeeResponses == null)
                {
                    return NoContent();
                }
                var employeeResponseList = new List<EmployeeDto>();
                foreach (var employeeResponse in employeeResponses)
                {
                    employeeResponseList.Add(new EmployeeDto
                    {
                        EmployeeId = employeeResponse.EmployeeId,
                        DateofBirth = employeeResponse.DateofBirth,
                        FirstName = employeeResponse.FirstName,
                        LastName = employeeResponse.LastName,
                        Department = employeeResponse.Department,
                        Designation = employeeResponse.Designation,
                        Unit = employeeResponse.Unit,
                        Mobile = employeeResponse.Mobile,
                        Email = employeeResponse.Email,
                        Password = employeeResponse.Password,
                        Other = employeeResponse.Other,
                        Agency = employeeResponse.Agency,
                        XEmployee = employeeResponse.XEmployee,
                    });

                }
                ApiResponse<List<EmployeeDto>> apiResponse = new ApiResponse<List<EmployeeDto>>()
                {
                    Status = "ok",
                    Message = "Employee List",
                    Result = employeeResponseList
                };
                var json = JsonConvert.SerializeObject(apiResponse, _serializerSettings);
                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return BadRequest();
            }
        }


        [HttpPut]
        [HttpPost]
        // // [Authorize(AuthenticationSchemes = "Bearer")]

        [Route("/api/employee/update")]
        public async Task<IActionResult> UpdateEmployeeAsync(EmployeeDto requestDto, CancellationToken cancellationToken)
        {
            ApiResponse<string> apiResponse = null;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;

                var createdBy = user.Id;
                var employeeResponse = await _masterService.FindEmployeeByIdAsync(requestDto.EmployeeId.ToString(), cancellationToken);

                if (employeeResponse == null)
                {
                    return NoContent();
                }

                employeeResponse.EmployeeId = (int)requestDto.EmployeeId;
                employeeResponse.FirstName = requestDto.FirstName;
                employeeResponse.LastName = requestDto.LastName;
                employeeResponse.Department = requestDto.Department;
                employeeResponse.Designation = requestDto.Designation;
                employeeResponse.Unit = requestDto.Unit;
                employeeResponse.Mobile = requestDto.Mobile;
                employeeResponse.Email = requestDto.Email;
                employeeResponse.Password = requestDto.Password;
                employeeResponse.Other = requestDto.Other;
                employeeResponse.Agency = requestDto.Agency;
                employeeResponse.Gender = requestDto.Gender;
                employeeResponse.XEmployee = requestDto.XEmployee;
                employeeResponse.DateofBirth = requestDto.DateofBirth;
                employeeResponse.CreatedBy = createdBy.ToString();
                employeeResponse.UpdatedBy= createdBy.ToString();
                var response = await _masterService.UpdateEmployeeAsync(employeeResponse, cancellationToken);
                if (response)
                {
                    apiResponse = new ApiResponse<string>()
                    {
                        Status = "ok",
                        Message = "Employee Updated Successfully",
                        Result = ""
                    };
                }
                else
                {
                    throw new Exception("Employee update failed");
                }

                var json = JsonConvert.SerializeObject(apiResponse, _serializerSettings);
                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<string>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = ""
                };
                return BadRequest(apiResponse);
            }
        }


        [HttpPost]
        //// [Authorize(AuthenticationSchemes = "Bearer")]

        [Route("/api/employee/delete")]
        public async Task<IActionResult> DeleteEmployeeAsync([FromQuery] int id)
        {

            try
            {
                var productResponse = await _masterService.DeleteEmployeeAsync(id);

                ApiResponse<string> apiResponse = new ApiResponse<string>()
                {
                    Status = "ok",
                    Message = "Employee Delete Successfully",
                    Result = ""
                };

                var json = JsonConvert.SerializeObject(apiResponse, _serializerSettings);
                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return BadRequest();
            }
        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 04/03/2023
        /// To bulk import empoyee
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/employee/import")]
        public async Task<IActionResult> BulkEmployeeCreate(IFormFile file, CancellationToken cancellationToken)
        {
            ApiResponse<FileStreamResult> apiResponse = new ApiResponse<FileStreamResult>();
            try
            {

                IExcelDataReader reader;
                MemoryStream stream = new MemoryStream();
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;
                List<Employee> lstProcessedEmployee = new List<Employee>();

                var createdBy = user.Id;
                // Check the File is received

                if (file == null)
                    throw new Exception("File is Not Received...");
                file.CopyTo(stream);

                // MAke sure that only Excel file is used 
                //string dataFileName = Path.GetFileName(file.FileName);

                string extension = Path.GetExtension(file.FileName);

                string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

                if (!allowedExtsnions.Contains(extension))
                    throw new Exception("Sorry! This file is not allowed, make sure that file having extension as either.xls or.xlsx is uploaded.");


                // USe this to handle Encodeing differences in .NET Core
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                if (extension == ".xls")
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                else
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                DataSet ds = new DataSet();
                ds = reader.AsDataSet();
                reader.Close();

                if (ds != null && ds.Tables.Count > 0)
                {
                    // Read the the Table
                    DataTable serviceDetails = ds.Tables[0];
                    for (int i = 1; i < serviceDetails.Rows.Count; i++)
                    {
                        DateTime dob;
                        System.DateTime.TryParse(Convert.ToString(serviceDetails.Rows[i][12]), out dob);
                        var employee = new Employee();
                        employee.FirstName = Convert.ToString(serviceDetails.Rows[i][0]);
                        employee.LastName = Convert.ToString(serviceDetails.Rows[i][1]);
                        employee.Department = Convert.ToString(serviceDetails.Rows[i][2]);
                        employee.Designation = Convert.ToString(serviceDetails.Rows[i][3]);
                        employee.Unit = Convert.ToString(serviceDetails.Rows[i][4]);
                        employee.Mobile = Convert.ToString(serviceDetails.Rows[i][5]);
                        employee.Email = Convert.ToString(serviceDetails.Rows[i][6]);
                        employee.Password = Convert.ToString(serviceDetails.Rows[i][8].ToString());
                        employee.Other = Convert.ToString(serviceDetails.Rows[i][7]);
                        employee.Agency = Convert.ToString(serviceDetails.Rows[i][9]);
                        employee.XEmployee = Convert.ToBoolean(serviceDetails.Rows[i][10].ToString());
                        employee.Gender = Convert.ToInt32(Convert.ToString(serviceDetails.Rows[i][11]));
                        employee.DateofBirth = dob;// = Convert.ToDateTime(Convert.ToString(serviceDetails.Rows[i][12])),
                        employee.CreatedBy = createdBy.ToString();
                        //employee.UserId = createdBy;
                        employee.CreatedDateTimeUtc = System.DateTime.UtcNow;
                        employee.UpdatedBy = createdBy.ToString();
                        employee.UpdatedDateTimeUtc = System.DateTime.UtcNow;
                        
                        if (string.IsNullOrEmpty(employee.Email) || string.IsNullOrEmpty(employee.Password) || string.IsNullOrEmpty(employee.FirstName))
                        {
                            employee.Remark = "Employee creation failed," + (string.IsNullOrEmpty(employee.Email) ? "Email " : string.IsNullOrEmpty(employee.Password) ? "Password" : "FirstName") + " is required.";
                            lstProcessedEmployee.Add(employee);
                            continue;
                        }
                        try
                        {
                            employee = await _masterService.CreateEmployeeAsync(employee, _userManager, cancellationToken);
                            employee.Remark = "Success";
                        }
                        catch (Exception e)
                        {
                            employee.Remark = e.Message;
                        }

                        lstProcessedEmployee.Add(employee);
                    }
                }
                var dt = lstProcessedEmployee.ToDataTable<Employee>();
                return (await ExportToExcel(dt, file.FileName));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<FileStreamResult>()
                {
                    Status = "ok",
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(apiResponse);
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 04/03/2023
        /// To return imported empoyee response
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/employee/export")]
        public async Task<IActionResult> ExportToExcel(DataTable table, string fileName)
        {

            var memoryStream = new MemoryStream();
            IWorkbook workbook = new XSSFWorkbook();
            using (var fs = new FileStream(Path.Combine(fileName), FileMode.Create, FileAccess.Write))
            {
                ISheet excelSheet = workbook.CreateSheet("Sheet1");

                List<String> columns = new List<string>();
                IRow row = excelSheet.CreateRow(0);
                int columnIndex = 0;

                foreach (System.Data.DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);
                    row.CreateCell(columnIndex).SetCellValue(column.ColumnName);
                    columnIndex++;
                }

                int rowIndex = 1;
                foreach (DataRow dsrow in table.Rows)
                {
                    row = excelSheet.CreateRow(rowIndex);
                    int cellIndex = 0;
                    foreach (String col in columns)
                    {
                        row.CreateCell(cellIndex).SetCellValue(dsrow[col].ToString());
                        cellIndex++;
                    }

                    rowIndex++;
                }
                workbook.Write(fs);
            }
            using (var fileStream = new FileStream(Path.Combine(fileName), FileMode.Open))
            {
                await fileStream.CopyToAsync(memoryStream);
            }
            memoryStream.Position = 0;
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 04/03/2023
        /// To download a sample empoyee excel
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/employee/downloadsample")]
        public async Task<IActionResult> DownloadSampleExcel()
        {
            try
            {
                var filepath = Path.Combine(hostEnvironment.WebRootPath, "Samples", "EmployeeUploadSample.xlsx");
                return File(System.IO.File.ReadAllBytes(filepath), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", System.IO.Path.GetFileName(filepath));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                var apiResponse = new ApiResponse<string>()
                {
                    Status = "ok",
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(apiResponse);
            }          
           
        }

        #endregion

        #region Unit Master

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 02/03/2023
        /// To Add Unit master  
        /// </summary>
        /// <param name="unitMaster"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/unit/create")]
        public async Task<IActionResult> CreateUnitMaster([FromBody] Unit unitMaster, CancellationToken cancellationToken)
        {
            ApiResponse<Unit> apiResponse = null;
            Unit unitMasterResponse = null;
            try
            {
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;

                var createdBy = user.Id;
                if (unitMaster != null)
                {
                    unitMaster.CreatedBy = createdBy;
                    unitMaster.ModifiedBy = createdBy;
                }

                unitMasterResponse = await _masterService.CreateUnitAsync(unitMaster, _userManager, cancellationToken);
                apiResponse = new ApiResponse<Unit>()
                {
                    Status = "ok",
                    Message = "unit Master Created Successfully",
                    Result = unitMasterResponse
                };
                return Ok(apiResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<Unit>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = unitMasterResponse
                };
                return BadRequest(apiResponse);
            }
        }
       
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 02/03/2023
        /// To get list of Unit master  
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/unit/getall")]
        public async Task<IActionResult> GetAllUnitAsync()
        {
            ApiResponse<List<UnitDetails>> apiResponse = null;
            var unitResponseList = new List<UnitDetails>();
            try
            {
                unitResponseList = await _masterService.GetAllUnitAsync(new Unit());
                if (unitResponseList == null)
                {
                    return NoContent();
                }

                apiResponse = new ApiResponse<List<UnitDetails>>()
                {
                    Status = "ok",
                    Message = "Unit List",
                    Result = unitResponseList
                };
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<List<UnitDetails>>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = unitResponseList
                };
                return BadRequest(apiResponse);
            }

        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 02/03/2023
        /// To get  unit master  by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/unit/{id}")]
        public async Task<IActionResult> GetUnitByIdAsync(int id, CancellationToken cancellationToken)
        {
            ApiResponse<UnitDetails> apiResponse = null;
            var unitResponse = new UnitDetails();
            try
            {
                Unit unit = new Unit() { UnitId = id };
                var response = await _masterService.GetAllUnitAsync(unit);
                if (response == null)
                {
                    return NoContent();
                }
                else
                {
                    unitResponse = response.FirstOrDefault();
                }

                apiResponse = new ApiResponse<UnitDetails>()
                {
                    Status = "ok",
                    Message = "",
                    Result = unitResponse
                };
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<UnitDetails>()
                {
                    Status = "ok",
                    Message = "",
                    Result = unitResponse
                };
                return BadRequest(apiResponse);
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 02/03/2023
        /// To Add update  unit master  
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [HttpPost]
        [Route("/api/unit/update")]
        public async Task<IActionResult> UpdateUnitAsync([FromForm] UnitUpdate requestDto, CancellationToken cancellationToken)
        {
            ApiResponse<Unit> apiResponse = null;
            var unitResponse = new Unit();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;
                requestDto.ModifiedBy = user.Id;
                Unit unit = new Unit()
                {
                    UnitCode = requestDto.UnitCode,
                    UnitDisplayName = requestDto.UnitDisplayName,
                    UnitEHSAdmin = requestDto.UnitEHSAdmin,
                    UnitEHSHead = requestDto.UnitEHSHead,
                    UnitHead = requestDto.UnitHead,
                    UnitId = requestDto.UnitId,
                    UnitStatus = requestDto.UnitStatus,
                    UnitUserName = requestDto.UnitUserName,
                    UnitUserPassword = requestDto.UnitUserPassword,
                    CreatedBy = requestDto.CreatedBy,
                    DivisionId = requestDto.DivisionId,
                    Modoules = requestDto.Modoules,
                    ModifiedBy = requestDto.ModifiedBy
                };
                if (requestDto.MobileFile != null)
                {
                    string dirPath = Path.Combine(hostEnvironment.WebRootPath, "Uploads/" + unit.UnitId.ToString() + "/MobileFiles");
                    //Create the Directory if it is not exist
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    string mobileFileName = Path.GetFileName(requestDto.MobileFile.FileName);
                    // Make a Copy of the Posted File from the Received HTTP Request
                    string saveToPath = Path.Combine(dirPath, mobileFileName);
                    using (FileStream stream = new FileStream(saveToPath, FileMode.Create))
                    {
                        requestDto.MobileFile.CopyTo(stream);
                    }
                    unit.MobileFile = mobileFileName;
                }

                if (requestDto.WebFile != null)
                {
                    string dirPath = Path.Combine(hostEnvironment.WebRootPath, "Uploads/"+ unit.UnitId.ToString()+"/ WebFiles");
                    //Create the Directory if it is not exist
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    string webaFileName = Path.GetFileName(requestDto.WebFile.FileName);
                    // Make a Copy of the Posted File from the Received HTTP Request
                    string saveToPath = Path.Combine(dirPath, webaFileName);
                    using (FileStream stream = new FileStream(saveToPath, FileMode.Create))
                    {
                        requestDto.WebFile.CopyTo(stream);
                    }
                    unit.WebFile = webaFileName;
                }


                unitResponse = await _masterService.CreateUnitAsync(unit, _userManager, cancellationToken);

                apiResponse = new ApiResponse<Unit>()
                {
                    Status = "ok",
                    Message = "unit master updated successfully",
                    Result = unitResponse
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<Unit>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = unitResponse
                };
                return BadRequest(apiResponse);
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 02/03/2023
        /// To delete  unit/plant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("/api/unit/delete")]
        public async Task<IActionResult> DeleteUnitAsync([FromQuery] int id)
        {
            ApiResponse<Unit> apiResponse = null;
            var unitResponseList = new Unit();

            try
            {
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;
                Unit unit = new Unit() { ModifiedBy = user.Id, UnitId = id };

                unitResponseList = await _masterService.DeleteUnitAsync(unit);

                apiResponse = new ApiResponse<Unit>()
                {
                    Status = "ok",
                    Message = "unit delete successfully",
                    Result = unitResponseList,
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<Unit>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = unitResponseList,
                };
                return BadRequest(apiResponse);
            }
        }
        #endregion
        #region Modules

        /// <summary>
        /// To get allactive modules
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/modules/getall")]
        public async Task<IActionResult> GetAllMoules()
        {
            ApiResponse<List<Modules>> apiResponse = new ApiResponse<List<Modules>>();
            List<Modules> lstModules = new List<Modules>();
            try
            {
                lstModules = await _masterService.GetAllModulesAsync();

                apiResponse = new ApiResponse<List<Modules>>()
                {
                    Status = "ok",
                    Message = "",
                    Result = lstModules,
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<List<Modules>>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = lstModules,
                };
                return BadRequest(apiResponse);
            }

        }
        #endregion

        #region Department Master

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To Add department Master
        /// </summary>
        /// <param name="departmentMaster"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/department/create")]
        public async Task<IActionResult> CreateDepartmentMaster([FromBody] Department departmentMaster, CancellationToken cancellationToken)
        {
            ApiResponse<Department> apiResponse = null;
            Department departmentMasterResponse = null;
            try
            {
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;

                var createdBy = user.Id;
                if (departmentMaster != null)
                {
                    departmentMaster.CreatedBy = createdBy;
                    departmentMaster.ModifyBy = createdBy;
                }

                departmentMasterResponse = await _masterService.AddUpdateDepartmentAsync(departmentMaster, cancellationToken);
                apiResponse = new ApiResponse<Department>()
                {
                    Status = "ok",
                    Message = "Department Created Successfully",
                    Result = departmentMasterResponse
                };
                return Ok(apiResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<Department>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = departmentMasterResponse
                };
                return BadRequest(apiResponse);
            }
        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To get list of department master  
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/department/getall")]
        public async Task<IActionResult> GetAllDepartmentAsync()
        {
            ApiResponse<List<DepartmentDetails>> apiResponse = null;
            var departmentResponseList = new List<DepartmentDetails>();
            try
            {
                departmentResponseList = await _masterService.GetAllDepartmentsAsync(new Department());
                if (departmentResponseList == null)
                {
                    return NoContent();
                }

                apiResponse = new ApiResponse<List<DepartmentDetails>>()
                {
                    Status = "ok",
                    Message = "Department List",
                    Result = departmentResponseList
                };
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<List<DepartmentDetails>>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = departmentResponseList
                };
                return BadRequest(apiResponse);
            }

        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 02/03/2023
        /// To get  unit master  by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/department/{id}")]
        public async Task<IActionResult> GetDepartmentByIdAsync(int id, CancellationToken cancellationToken)
        {
            ApiResponse<DepartmentDetails> apiResponse = null;
            var dptResponse = new DepartmentDetails();
            try
            {
                Department department = new Department() { DepartmentId = id };
                var response = await _masterService.GetAllDepartmentsAsync(department);
                if (response == null)
                {
                    return NoContent();
                }
                else
                {
                    dptResponse = response.FirstOrDefault();
                }

                apiResponse = new ApiResponse<DepartmentDetails>()
                {
                    Status = "ok",
                    Message = "",
                    Result = dptResponse
                };
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<DepartmentDetails>()
                {
                    Status = "ok",
                    Message = "",
                    Result = dptResponse
                };
                return BadRequest(apiResponse);
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To Add update  unit master  
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [HttpPost]
        [Route("/api/department/update")]
        public async Task<IActionResult> UpdateDepartmentAsync(Department requestDto, CancellationToken cancellationToken)
        {
            ApiResponse<Department> apiResponse = null;
            var unitResponse = new Department();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;
                requestDto.ModifyBy = user.Id;
                


                unitResponse = await _masterService.AddUpdateDepartmentAsync(requestDto, cancellationToken);

                apiResponse = new ApiResponse<Department>()
                {
                    Status = "ok",
                    Message = "Department master updated successfully",
                    Result = unitResponse
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<Department>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = unitResponse
                };
                return BadRequest(apiResponse);
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 02/03/2023
        /// To delete  unit/plant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("/api/department/delete")]
        public async Task<IActionResult> DeleteDepartmentAsync([FromQuery] int id)
        {
            ApiResponse<Department> apiResponse = null;
            var deaprtmentResponse = new Department();

            try
            {
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;
                Department department = new Department() { ModifyBy = user.Id, DepartmentId = id };

                deaprtmentResponse = await _masterService.DeleteDepartmentAsync(department);

                apiResponse = new ApiResponse<Department>()
                {
                    Status = "ok",
                    Message = "department deleted successfully",
                    Result = deaprtmentResponse,
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<Department>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = deaprtmentResponse,
                };
                return BadRequest(apiResponse);
            }
        }
        #endregion

        #region Department Section Master

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 07/03/2023
        /// To Add department Section Master
        /// </summary>
        /// <param name="departmentSectionMaster"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/section/create")]
        public async Task<IActionResult> CreateDepartmentSectionMaster([FromBody] Sections departmentSectionMaster, CancellationToken cancellationToken)
        {
            ApiResponse<Sections> apiResponse = null;
            Sections sectionMasterResponse = null;
            try
            {
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;

                var createdBy = user.Id;
                if (departmentSectionMaster != null)
                {
                    departmentSectionMaster.CreatedBy = createdBy;
                    departmentSectionMaster.ModifyBy = createdBy;
                }

                sectionMasterResponse = await _masterService.AddUpdateDepartmentSectionAsync(departmentSectionMaster, cancellationToken);
                apiResponse = new ApiResponse<Sections>()
                {
                    Status = "ok",
                    Message = "Department Section Created Successfully",
                    Result = sectionMasterResponse
                };
                return Ok(apiResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<Sections>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = sectionMasterResponse
                };
                return BadRequest(apiResponse);
            }
        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 07/03/2023
        /// To get list of department sections   
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/section/getall")]
        public async Task<IActionResult> GetAllDepartmentSectionsAsync()
        {
            ApiResponse<List<DepartmentSectionDetails>> apiResponse = null;
            var departmentResponseList = new List<DepartmentSectionDetails>();
            try
            {
                departmentResponseList = await _masterService.GetAllDepartmentSectionAsync(new Sections());
                if (departmentResponseList == null)
                {
                    return NoContent();
                }

                apiResponse = new ApiResponse<List<DepartmentSectionDetails>>()
                {
                    Status = "ok",
                    Message = "Sections List",
                    Result = departmentResponseList
                };
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<List<DepartmentSectionDetails>>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = departmentResponseList
                };
                return BadRequest(apiResponse);
            }

        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 07/03/2023
        /// To get  section master  by id
        /// </summary>
        /// <param name="id"></param>      
        /// <returns></returns>
        [HttpGet]
        [Route("/api/section/{id}")]
        public async Task<IActionResult> GetDepartmentSectionByIdAsync(int id)
        {
            ApiResponse<List<DepartmentSectionDetails>> apiResponse = null;
            var sectionResponse = new List<DepartmentSectionDetails>();
            try
            {
                Sections section = new Sections() { DepartmentSectionId = id };
                var response = await _masterService.GetAllDepartmentSectionAsync(section);
                if (response == null)
                {
                    return NoContent();
                }
                else
                {
                    sectionResponse = response;
                }

                apiResponse = new ApiResponse<List<DepartmentSectionDetails>>()
                {
                    Status = "ok",
                    Message = "",
                    Result = sectionResponse
                };
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<List<DepartmentSectionDetails>>()
                {
                    Status = "ok",
                    Message = "",
                    Result = sectionResponse
                };
                return BadRequest(apiResponse);
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 07/03/2023
        /// To Add section master  
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [HttpPost]
        [Route("/api/section/update")]
        public async Task<IActionResult> UpdateDepartmentSectionAsync(Sections requestDto, CancellationToken cancellationToken)
        {
            ApiResponse<Sections> apiResponse = null;
            var sectionResponse = new Sections();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;
                requestDto.ModifyBy = user.Id;



                sectionResponse = await _masterService.AddUpdateDepartmentSectionAsync(requestDto, cancellationToken);

                apiResponse = new ApiResponse<Sections>()
                {
                    Status = "ok",
                    Message = "section master updated successfully",
                    Result = sectionResponse
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<Sections>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = sectionResponse
                };
                return BadRequest(apiResponse);
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 02/03/2023
        /// To delete  section
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("/api/section/delete")]
        public async Task<IActionResult> DeleteDepartmentSectionAsync([FromQuery] int id)
        {
            ApiResponse<Sections> apiResponse = null;
            var sectionResponse = new Sections();

            try
            {
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;
                Sections section = new Sections() { ModifyBy = user.Id, DepartmentSectionId = id };

                sectionResponse = await _masterService.DeleteDepartmentSectionAsync(section);

                apiResponse = new ApiResponse<Sections>()
                {
                    Status = "ok",
                    Message = "section deleted successfully",
                    Result = sectionResponse,
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<Sections>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = sectionResponse,
                };
                return BadRequest(apiResponse);
            }
        }
        #endregion

        #region Responsible Person Master

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 07/03/2023
        /// To Add Responsible Person
        /// </summary>
        /// <param name="responsiblePersonMaster"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/responsibleperson/create")]
        public async Task<IActionResult> CreateResponsiblePerson([FromBody] ResponsiblePerson responsiblePersonMaster, CancellationToken cancellationToken)
        {
            ApiResponse<ResponsiblePerson> apiResponse = null;
            ResponsiblePerson responsiblePerson = null;
            try
            {
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;

                var createdBy = user.Id;
                if (responsiblePersonMaster != null)
                {
                    responsiblePersonMaster.CreatedBy = createdBy;
                    responsiblePersonMaster.ModifyBy = createdBy;
                }

                responsiblePerson = await _masterService.AddUpdateResponsiblePersonAsync(responsiblePersonMaster, cancellationToken);
                apiResponse = new ApiResponse<ResponsiblePerson>()
                {
                    Status = "ok",
                    Message = "Responsible person created Successfully",
                    Result = responsiblePerson
                };
                return Ok(apiResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<ResponsiblePerson>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = responsiblePerson
                };
                return BadRequest(apiResponse);
            }
        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 07/03/2023
        /// To get list of Responsible Persons   
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/responsibleperson/getall")]
        public async Task<IActionResult> GetAllResponsiblePersonAsync()
        {
            ApiResponse<List<ResponsiblePersonDetails>> apiResponse = null;
            var responsiblePersonList = new List<ResponsiblePersonDetails>();
            try
            {
                responsiblePersonList = await _masterService.GetAllResponsiblePersonAsync(new ResponsiblePerson());
                if (responsiblePersonList == null)
                {
                    return NoContent();
                }

                apiResponse = new ApiResponse<List<ResponsiblePersonDetails>>()
                {
                    Status = "ok",
                    Message = "Responsible Person List",
                    Result = responsiblePersonList
                };
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<List<ResponsiblePersonDetails>>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = responsiblePersonList
                };
                return BadRequest(apiResponse);
            }

        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 07/03/2023
        /// To get  Responsible Person by id
        /// </summary>
        /// <param name="id"></param>      
        /// <returns></returns>
        [HttpGet]
        [Route("/api/responsibleperson/{id}")]
        public async Task<IActionResult> GetResponsiblePersonByIdAsync(int id)
        {
            ApiResponse<ResponsiblePersonDetails> apiResponse = null;
            var responsiblePerson = new ResponsiblePersonDetails();
            try
            {
                ResponsiblePerson section = new ResponsiblePerson() { ResponsibleId = id };
                var response = await _masterService.GetAllResponsiblePersonAsync(section);
                if (response == null)
                {
                    return NoContent();
                }
                else
                {
                    responsiblePerson = response.FirstOrDefault();
                }

                apiResponse = new ApiResponse<ResponsiblePersonDetails>()
                {
                    Status = "ok",
                    Message = "",
                    Result = responsiblePerson
                };
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<ResponsiblePersonDetails>()
                {
                    Status = "ok",
                    Message = "",
                    Result = responsiblePerson
                };
                return BadRequest(apiResponse);
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 07/03/2023
        /// To update Responsible Person   
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [HttpPost]
        [Route("/api/responsibleperson/update")]
        public async Task<IActionResult> UpdateDepartmentSectionAsync(ResponsiblePerson requestDto, CancellationToken cancellationToken)
        {
            ApiResponse<ResponsiblePerson> apiResponse = null;
            var responsiblePerson = new ResponsiblePerson();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;
                requestDto.ModifyBy = user.Id;

                responsiblePerson = await _masterService.AddUpdateResponsiblePersonAsync(requestDto, cancellationToken);

                apiResponse = new ApiResponse<ResponsiblePerson>()
                {
                    Status = "ok",
                    Message = "responsible person updated successfully",
                    Result = responsiblePerson
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<ResponsiblePerson>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = responsiblePerson
                };
                return BadRequest(apiResponse);
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 07/03/2023
        /// To delete  responsible Person
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("/api/responsibleperson/delete")]
        public async Task<IActionResult> DeleteResponsiblePersonAsync([FromQuery] int id)
        {
            ApiResponse<ResponsiblePerson> apiResponse = null;
            var responsiblePerson = new ResponsiblePerson();

            try
            {
                var user = Request.HttpContext.Items["CurrentLoggedUser"] as ApplicationUser;
                ResponsiblePerson person = new ResponsiblePerson() { ModifyBy = user.Id, ResponsibleId = id };

                responsiblePerson = await _masterService.DeleteResponsiblePersonAsync(person);

                apiResponse = new ApiResponse<ResponsiblePerson>()
                {
                    Status = "ok",
                    Message = "Responsible Person deleted successfully",
                    Result = responsiblePerson,
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<ResponsiblePerson>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = responsiblePerson,
                };
                return BadRequest(apiResponse);
            }
        }
        #endregion

        #region WorkPermitTypeDetail 
        
        /// <summary>
        /// PPPEType Master
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/PPPETypeMaster/getall")]
        public async Task<IActionResult> PPPETypeGetAllAsync()
        {
            ApiResponse<List<PPPEType>> apiResponse = null;
            var PPPETypeList = new List<PPPEType>();
            try
            {
                PPPETypeList = await _masterService.GetAllPPPETypesAsync(new PPPEType());
                if (PPPETypeList == null)
                {
                    return NoContent();
                }

                apiResponse = new ApiResponse<List<PPPEType>>()
                {
                    Status = "ok",
                    Message = "PPPE Type List",
                    Result = PPPETypeList
                };
                var json = JsonConvert.SerializeObject(apiResponse, _serializerSettings);
                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<List<PPPEType>>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = PPPETypeList
                };
                var json = JsonConvert.SerializeObject(apiResponse, _serializerSettings);                
                return BadRequest(json);
            }

        }

        /// <summary>
        /// Default Precautions Master
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/DefaultPrecautions/getall")]
        public async Task<IActionResult> GetAllDefaultPrecautionsAsync()
        {
            ApiResponse<List<Precaution>> apiResponse = null;
            var DefaultPrecautionsList = new List<Precaution>();
            try
            {
                DefaultPrecautionsList = await _masterService.GetAllDefaultPrecautionsAsync(0);
                if (DefaultPrecautionsList == null)
                {
                    return NoContent();
                }

                apiResponse = new ApiResponse<List<Precaution>>()
                {
                    Status = "ok",
                    Message = "PPPE Type List",
                    Result = DefaultPrecautionsList
                };

                var json = JsonConvert.SerializeObject(apiResponse, _serializerSettings);
                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                apiResponse = new ApiResponse<List<Precaution>>()
                {
                    Status = "failed",
                    Message = ex.Message,
                    Result = DefaultPrecautionsList
                };
                var json = JsonConvert.SerializeObject(apiResponse, _serializerSettings);
                return BadRequest(json);
            }

        }
        #endregion
    }
}
