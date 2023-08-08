using CRM.Server.Models;
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using EHS.Server.Models.Masters;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CRM.Server.Data
{
    public class UserRepo
    {
        private readonly string _connectionString;
        public UserRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = await conn.QueryAsync<ApplicationUser>("select Id,Gender,PhoneNumber,Email,FirstName,LastName,CreatedDateTimeUtc,UpdatedDateTimeUtc,Status from [dbo].[AspNetUsers]").ConfigureAwait(false);
                return result.ToList();
            }
        }
        public async Task<List<ApplicationUser>> GetAllUsersAsync(int RoleId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = await conn.QueryAsync<ApplicationUser>(@"select au.Id,au.Gender,au.FirstName,au.LastName,au.CreatedDateTimeUtc,au.UpdatedDateTimeUtc,au.[Status]  from aspnetUserroles aur inner join AspNetUsers au on aur.UserId = au.Id where aur.roleid = @RoleId
 ", new{ RoleId }).ConfigureAwait(false);
                return result.ToList();
            }
        }
        public async Task<IdentityResult> CreateUserEmployeeAsync(ApplicationUser user,string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user.Id > 0)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    int UserId = user.Id;
                    string Mobile = user.PhoneNumber;
                    string Password = password;
                    int EmployeeId = await connection.QuerySingleAsync<int>($@"INSERT INTO [dbo].[tbl_EmployeeMaster] ([FirstName],[LastName],[Mobile],[Email],[Password],[UserId])
                        VALUES (
                        @{nameof(user.FirstName)},
                        @{nameof(user.LastName)},                      
                        @{nameof(Mobile)},                
                        @{nameof(user.Email)},
                        @{nameof(Password)},                               
                        @{nameof(UserId)});
                        SELECT CAST(SCOPE_IDENTITY() as int)", new
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Mobile = user.PhoneNumber,
                        Email = user.Email,
                        Password = Password,
                        UserId = UserId
                    }).ConfigureAwait(false);
                
                }
            }

            return IdentityResult.Success;
        }
      
        
        /// <summary>
        /// this is currently not is use but when there is requirement for multiple units at time of creation we can use it.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<int> AddUpdateUserUnits(SqlConnection connection,ApplicationUser user)
        {
            // update Units
            var res = await connection.ExecuteAsync($@"                  
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
					,Target.ModifiedUTCDate = GETUTCDATE();", new
            {
                EmployeeId = user.Id,
                Units = user.PlantName,
                CreatedBy =0,
                ModifiedBy = 0
            });
            return res;
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 09/03/2023
        /// To get asigned units of an employee/user
        /// </summary>
        /// <param name="unserId"></param>        
        /// <returns></returns>
        public async Task<List<Unit>> GetUserUnitsByIdAsync(long unserId)
        {
            string CommamdText = "SELECT eu.[UnitId],[EmployeeId],[MobileFile],[WebFile] FROM [dbo].[tbl_EmployeeUnits] (NOLOCK) eu JOIN  [dbo].[tbl_UnitMaster] (NOLOCK) um ON eu.unitId =um.UnitId WHERE [EmployeeId]=@EmployeeId AND [IsActive]=1";
            List<Unit> lstUserUnits= new List<Unit>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);
                    var resposnse = await connection.QueryAsync<Unit>(CommamdText, param: new { EmployeeId = unserId }).ConfigureAwait(false);
                    lstUserUnits= resposnse.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lstUserUnits;
        }
    }
}
