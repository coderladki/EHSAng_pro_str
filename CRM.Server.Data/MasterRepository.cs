using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Threading;
using EHS.Server.Models.Masters;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System;
using DocumentFormat.OpenXml.Office2010.Excel;
using CRM.Server.Models;
using Microsoft.AspNetCore.Identity;
using CRM.Server.Models.Enum;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Reflection;

namespace EHS.Server.Data
{
    public class MasterRepository
    {

        private readonly string _connectionString;
        private readonly UserManager<ApplicationUser> _userManager;
        public MasterRepository(string connectionString)
        {
            _connectionString = connectionString;
          
        }

        public async Task<Division> CreateDivisionAsync(Division division, CancellationToken cancellationToken)
        {

            try { 
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                division.Id = await connection.QuerySingleAsync<int>($@"INSERT INTO [dbo].[Division] ([Name],[CreatedBy],[CreatedDateTimeUtc],[UpdatedBy],[UpdatedDateTimeUtc] )
                    VALUES (@{nameof(Division.Name)},
                    @{nameof(Division.CreatedBy)},
                    @{nameof(Division.CreatedDateTimeUtc)},
                    @{nameof(Division.UpdatedBy)},
                    @{nameof(Division.UpdatedDateTimeUtc)});
                    SELECT CAST(SCOPE_IDENTITY() as int)", division).ConfigureAwait(false);
            }
                return division;
            }
            catch(Exception ex)
            {
                return new Division();
            }

          
        }

        public async Task<List<Division>> GetAllDivisionAsync()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = await conn.QueryAsync<Division>("select * from [dbo].[Division]").ConfigureAwait(false);
                return result.ToList();
            }
        }

        public async Task<Division> FindDivisionByIdAsync(string Id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<Division>($@"SELECT * FROM [dbo].[Division]
                    WHERE [Id] = @{nameof(Id)}", new { Id }).ConfigureAwait(false);
            }
        }

        public async Task<Division> UpdateDivisionAsync(Division division, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($@"UPDATE [dbo].[Division] set 
                    [Name] =@{nameof(Division.Name)} 
                    WHERE [Id] = @{nameof(Division.Id)}", division).ConfigureAwait(false);
            }

            return null;
        }

        public async Task<Division> DeleteDivisionAsync(int id)
        {
            //cancellationToken.ThrowIfCancellationRequested();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);
                    await connection.ExecuteAsync($"DELETE FROM [dbo].[Division] WHERE [Id] = @{nameof(id)}", new { id = id }).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                 return new Division { Name = ex.Message };
            }
            return null;
        }

        #region Email Master
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 27/02/2023
        /// To create new email master
        /// </summary>
        /// <param name="master"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EmailsMaster> AddUpdateEmailMasterAsync(EmailsMaster master, CancellationToken cancellationToken)
        {

            try
            {
                string CommamdText = "usp_EmailMasters_AddUpdate";
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(cancellationToken);
                    master.EmailMasterId = await connection.ExecuteScalarAsync<int>(CommamdText,commandType:System.Data.CommandType.StoredProcedure, param:master);                  
                }
                return master;
            }
            catch (Exception)
            {
                throw;
            }


        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 27/02/2023
        /// To get email masters
        /// </summary>
        /// <param name="emailsMaster"></param>
        /// <returns></returns>
        public async Task<List<EmailsMaster>> GetAllEmailMasterAsync(EmailsMaster emailsMaster)
        {
            string CommamdText = "usp_EmailMasters_Get";
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var result = await conn.QueryAsync<EmailsMaster>(CommamdText, emailsMaster.EmailMasterId).ConfigureAwait(false);
                    return result.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 27/02/2023
        /// To delete  email master
        /// </summary>
        /// <param name="emailsMaster"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EmailsMaster> DeleteEmailMasterAsync(EmailsMaster emailsMaster)
        {
            string CommamdText = "usp_EmailMaster_Delete";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);
                    await connection.ExecuteAsync(CommamdText,commandType:System.Data.CommandType.StoredProcedure,param: new { EmailMasterId= emailsMaster.EmailMasterId, ModifiedBy= emailsMaster.ModifiedBy }).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region Employee Master

        public async Task<Employee> CreateEmployeeAsync(Employee employee, UserManager<ApplicationUser> _userManager, CancellationToken cancellationToken)
        {

            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(cancellationToken);

                    var user = new ApplicationUser
                    {
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        UserName = employee.Email,
                        Email = employee.Email,
                        LockoutEnabled = false,
                        TwoFactorEnabled = false,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        AccessFailedCount = 0,
                        PhoneNumber = employee.Mobile ?? "",
                        Status = 1,
                        CreatedDateTimeUtc = DateTime.UtcNow,
                        UpdatedDateTimeUtc = DateTime.UtcNow,
                        Gender = Enum.Parse<eGender>(employee.Gender.ToString()),
                        IsLoginEnabled= true,
                    };
                    var response = await _userManager.CreateAsync(user, employee.Password);
                    int UserId=user.Id;
                    employee.UserId=user.Id;
                    if (response.Succeeded)
                    {
                    employee.EmployeeId = await connection.QuerySingleAsync<int>($@"INSERT INTO [dbo].[tbl_EmployeeMaster] ([DateofBirth],[FirstName],[LastName],[Department],[Designation],[Mobile],[Email],[Password],[Other],[Agency],[XEmployee],[CreatedBy],[CreatedDateTimeUtc],[UpdatedBy],[UpdatedDateTimeUtc],[UserId],[Gender])
                        VALUES (@{nameof(employee.DateofBirth)},
                        @{nameof(employee.FirstName)},
                        @{nameof(employee.LastName)},
                        @{nameof(employee.Department)},
                        @{nameof(employee.Designation)},                     
                        @{nameof(employee.Mobile)},
                        @{nameof(employee.Email)},
                        @{nameof(employee.Password)},
                        @{nameof(employee.Other)},
                        @{nameof(employee.Agency)},
                        @{nameof(employee.XEmployee)},
                        @{nameof(employee.CreatedBy)},
                        @{nameof(employee.CreatedDateTimeUtc)},
                        @{nameof(employee.UpdatedBy)},
                        @{nameof(employee.UpdatedDateTimeUtc)},
                        @{nameof(employee.UserId)},
                        @{nameof(employee.Gender)});
                        SELECT CAST(SCOPE_IDENTITY() as int)", employee).ConfigureAwait(false);
                        employee.UserId = user.Id;  
                    } 
                    

                }
                return employee;
            }
            catch (Exception ex)
            {
                return new Employee();
            }


        }


        public async Task<List<Employee>> GetAllEmployeeAsync()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = await conn.QueryAsync<Employee>("select * from [dbo].[tbl_EmployeeMaster]").ConfigureAwait(false);
                return result.ToList();
            }
        }

        public async Task<Employee> FindEmployeeByIdAsync(string Id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<Employee>($@"SELECT * FROM [dbo].[tbl_EmployeeMaster]
                    WHERE [EmployeeId] = @{nameof(Id)}", new { Id }).ConfigureAwait(false);
            }
        }


        public async Task<bool> UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken)
        {
            int rowsEffected = 0;
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                rowsEffected = await connection.ExecuteAsync($@"UPDATE [dbo].[tbl_EmployeeMaster] set 
                    [FirstName] =@{nameof(Employee.FirstName)},
                    [DateofBirth] =@{nameof(Employee.DateofBirth)},
                    [LastName] =@{nameof(Employee.LastName)},
                    [Department] =@{nameof(Employee.Department)},
                    [Designation] =@{nameof(Employee.Designation)},                    
                    [Mobile] =@{nameof(Employee.Mobile)},
                    [Email] =@{nameof(Employee.Email)},
                    [Other] =@{nameof(Employee.Other)},
                    [Agency] =@{nameof(Employee.Agency)},
                    [Gender] =@{nameof(Employee.Gender)}
                    WHERE [EmployeeId] = @{nameof(Employee.EmployeeId)}", employee).ConfigureAwait(false);
                // update Units
                     var res=  await connection.ExecuteAsync($@"                  
	                   	SELECT @EmployeeId EmployeeId
				,value AS UnitId
				,1 IsActive
				,@CreatedBy CreatedBy
			INTO #temp
	    	FROM STRING_SPLIT(@Units, ',');

			MERGE tbl_EmployeeUnits AS Target
			USING #temp AS Source
				ON (
						Target.UnitId = Source.UnitId
						AND Target.EmployeeId = Source.EmployeeId
						)
			WHEN NOT MATCHED BY Target
				THEN
					INSERT (
						EmployeeId
						,UnitId
						,IsActive
						,CreatedBy
						,CreatedUTCDate
						)
					VALUES (
						Source.EmployeeId
						,Source.UnitId
						,Source.IsActive
						,@CreatedBy
						,GETUTCDATE()
						)
			WHEN MATCHED
				THEN
					UPDATE
					SET Target.EmployeeId = Source.EmployeeId
						,Target.UnitId = Source.UnitId
						,Target.IsActive = 1
						,Target.ModifiedBy = @CreatedBy
						,Target.ModifiedUTCDate = GETUTCDATE()
			WHEN NOT MATCHED BY Source AND Target.EmployeeId=@EmployeeId
				THEN
					UPDATE
					SET Target.IsActive = 0
					,Target.ModifiedBy = @CreatedBy
					,Target.ModifiedUTCDate = GETUTCDATE();", new { EmployeeId = employee.EmployeeId, Units = employee.Unit, CreatedBy = employee.CreatedBy, ModifiedBy=employee.CreatedBy });
            }

           return rowsEffected>0?true:false;
        }

        public async Task<Employee> DeleteEmployeeAsync(int id)
        {
            //cancellationToken.ThrowIfCancellationRequested();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);
                    await connection.ExecuteAsync($"DELETE FROM [dbo].[tbl_EmployeeMaster] WHERE [EmployeeId] = @{nameof(id)}", new { id = id }).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                return new Employee { FirstName = ex.Message };
            }
            return null;
        }



        #endregion
        #region Unit Master
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 02/03/2023
        /// To Add or update unit/plant
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Unit> CreateUnitAsync(Unit unit, UserManager<ApplicationUser> _userManager, CancellationToken cancellationToken)
        {
            try
            {
                string CommamdText = "usp_UnitMaster_AddUpDate";
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(cancellationToken);
                    var PlantResponse = await connection.QueryAsync<dynamic>(CommamdText, commandType: System.Data.CommandType.StoredProcedure, param: unit);
                    var _PlantResponse = PlantResponse.FirstOrDefault();
                    if (_PlantResponse != null)
                    {
                        var user = new ApplicationUser
                        {
                            FirstName = unit.UnitUserName,
                            LastName = unit.UnitUserName,
                            UserName = unit.UnitUserName,
                            Email = unit.UnitUserName,
                            LockoutEnabled = false,
                            TwoFactorEnabled = false,
                            EmailConfirmed = true,
                            PhoneNumberConfirmed = true,
                            AccessFailedCount = 0,
                            PhoneNumber = "",
                            Status = 1,
                            CreatedDateTimeUtc = DateTime.UtcNow,
                            UpdatedDateTimeUtc = DateTime.UtcNow,
                            Gender = Enum.Parse<eGender>(eGender.Others.ToString()),
                            PlantId = unit.UnitId,
                            Id = _PlantResponse.UserId,
                            IsLoginEnabled= true,
                        };
                        if (_PlantResponse.UnitId > 0 && unit.UnitId == 0)
                        {
                            var response = await _userManager.CreateAsync(user, unit.UnitUserPassword);
                        }
                        else
                        {
                            var response = await _userManager.UpdateAsync(user);
                        }
                    }


                }
                return unit;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 02/03/2023
        /// To get all unit details
        /// </summary>
        /// <param name="emailsMaster"></param>
        /// <returns></returns>
        public async Task<List<UnitDetails>> GetAllUnitAsync(Unit unit)
        {
            string CommamdText = "usp_UnitMaster_GetAll";
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var result = await conn.QueryAsync<UnitDetails>(CommamdText, unit.UnitId).ConfigureAwait(false);
                    return result.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 02/03/2023
        /// To delete  unit
        /// </summary>
        /// <param name="unit"></param>        
        /// <returns></returns>
        public async Task<Unit> DeleteUnitAsync(Unit unit)
        {
            string CommamdText = "usp_UnitMaster_Delete";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);
                    await connection.ExecuteAsync(CommamdText, commandType: System.Data.CommandType.StoredProcedure, param: new { UnitId = unit.UnitId, ModifiedBy = unit.ModifiedBy }).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion
        #region Modules
        /// <summary>
        /// To get all modules
        /// </summary>
        /// <returns></returns>
        public async Task<List<Modules>> GetAllModulesAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {                    
                   var responce =await connection.QueryAsync<Modules>("SELECT ModuleId,ModuleName,Status FROM tbl_ModuleMaster WHERE Status=1").ConfigureAwait(false);
                    return responce.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        #region Department Master
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To Add or update department
        /// </summary>
        /// <param name="department"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Department> AddUpdateDepartmentAsync(Department department, CancellationToken cancellationToken)
        {
            try
            {
                string CommamdText = "usp_DepartmentMaster_AddUpDate";
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(cancellationToken);
                    department.DepartmentId = await connection.ExecuteScalarAsync<int>(CommamdText, commandType: System.Data.CommandType.StoredProcedure, param: department);
                }
                return department;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To get all departments
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public async Task<List<DepartmentDetails>> GetAllDepartmentsAsync(Department department)
        {
            string CommamdText = "usp_DepartmentMaster_GetALL";
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var result = await conn.QueryAsync<DepartmentDetails>(CommamdText, department.DepartmentId).ConfigureAwait(false);
                    return result.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To delete  department
        /// </summary>
        /// <param name="department"></param>        
        /// <returns></returns>
        public async Task<Department> DeleteDepartmentAsync(Department department)
        {
            string CommamdText = "usp_DepartmentMaster_Delete";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);
                    await connection.ExecuteAsync(CommamdText, commandType: System.Data.CommandType.StoredProcedure, param: new { DepartmentId = department.DepartmentId, ModifyBy = department.ModifyBy }).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region Department Section Master
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To Add or update department Section
        /// </summary>
        /// <param name="sections"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Sections> AddUpdateDepartmentSectionAsync(Sections sections, CancellationToken cancellationToken)
        {
            try
            {
                string CommamdText = "usp_SectionMaster_AddUpDate";
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(cancellationToken);
                    sections.DepartmentSectionId = await connection.ExecuteScalarAsync<int>(CommamdText, commandType: System.Data.CommandType.StoredProcedure, param: sections);
                }
                return sections;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To get all sections
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public async Task<List<DepartmentSectionDetails>> GetAllDepartmentSectionAsync(Sections sections)
        {
            string CommamdText = "usp_DepartmentSectionMaster_GetALL";
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var result = await conn.QueryAsync<DepartmentSectionDetails>(CommamdText, sections.DepartmentId).ConfigureAwait(false);
                    return result.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To delete  department section
        /// </summary>
        /// <param name="sections"></param>        
        /// <returns></returns>
        public async Task<Sections> DeleteDepartmentSectionAsync(Sections sections)
        {
            string CommamdText = "usp_DepartmentSectionMaster_Delete";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);
                    await connection.ExecuteAsync(CommamdText, commandType: System.Data.CommandType.StoredProcedure, param: new { DepartmentSectionId = sections.DepartmentSectionId, ModifyBy = sections.ModifyBy }).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region Responsible persons Department
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To Add or update Responsible Person
        /// </summary>
        /// <param name="person"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponsiblePerson> AddUpdateResponsiblePersonAsync(ResponsiblePerson person, CancellationToken cancellationToken)
        {
            try
            {
                string CommamdText = "usp_ResponsiblePerson_AddUpDate";
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(cancellationToken);
                    person.ResponsibleId = await connection.ExecuteScalarAsync<int>(CommamdText, commandType: System.Data.CommandType.StoredProcedure, param: person);
                }
                return person;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To get all person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task<List<ResponsiblePersonDetails>> GetAllResponsiblePersonAsync(ResponsiblePerson person)
        {
            string CommamdText = "usp_ResponsiblePerson_GetALL";
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var result = await conn.QueryAsync<ResponsiblePersonDetails>(CommamdText, person.ResponsibleId).ConfigureAwait(false);
                    return result.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To delete  person
        /// </summary>
        /// <param name="person"></param>        
        /// <returns></returns>
        public async Task<ResponsiblePerson> DeleteResponsiblePersonAsync(ResponsiblePerson person)
        {
            string CommamdText = "usp_ResponsiblePerson_Delete";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);
                    await connection.ExecuteAsync(CommamdText, commandType: System.Data.CommandType.StoredProcedure, param: new { ResponsibleId = person.ResponsibleId, ModifyBy = person.ModifyBy }).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region WorkPermitTypeDetail
        /// <summary>
        /// Author: Preeti Juyal
        /// Created Date: 08/03/2023
        /// Get all PPPETypes from master if pppetype is null else pppeType details with given PppeType Id
        /// </summary>
        /// <param name="pppeType"></param>
        /// <returns></returns>
        public async Task<List<PPPEType>> GetAllPPPETypesAsync(PPPEType pppeType)
        {
            string CommamdText = "usp_PPPETypeMaster_GetALL";
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var result = await conn.QueryAsync<PPPEType>(CommamdText, pppeType.PppeTypeId).ConfigureAwait(false);
                    return result.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Author: Preeti Juyal
        /// Created Date : 08/03/2023
        /// Get default precautions master if precautionId is not provided else the Default Precaution with given Id
        /// </summary>
        /// <param name="precautionId"></param>
        /// <returns></returns>
        public async Task<List<Precaution>> GetAllDefaultPrecautionsAsync(int precautionId)
        {
            string CommamdText = "usp_DefaultPrecautionsMaster_GetALL";
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var result = await conn.QueryAsync<Precaution>(CommamdText, precautionId).ConfigureAwait(false);
                    return result.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

    }
}
