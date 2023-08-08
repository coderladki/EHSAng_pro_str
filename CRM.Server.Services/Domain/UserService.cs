using CRM.Server.Data;
using CRM.Server.Models;
using Dapper;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using EHS.Server.Data;
using EHS.Server.Models.Masters;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CRM.Server.Services.Domain
{
    public class UserService
    {
        private readonly UserRepo _userRepo;
        public UserService(string connectionString)
        {
            _userRepo = new UserRepo(connectionString);
        }

        public async Task<List<ApplicationUser>> GetAllUserAsync()
        {
            return await _userRepo.GetAllUsersAsync().ConfigureAwait(false);
        }
        public async Task<List<ApplicationUser>> GetAllUserByRoleIdAsync(int RoleId)
        {
            return await _userRepo.GetAllUsersAsync(RoleId).ConfigureAwait(false);
        }
        public async Task<IdentityResult> CreateUserEmployeeAsync(ApplicationUser user, string password, CancellationToken cancellationToken)
        {

            return await _userRepo.CreateUserEmployeeAsync(user, password, cancellationToken).ConfigureAwait(false);

        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 09/03/2023
        /// To get asigned units of an employee
        /// </summary>
        /// <param name="person"></param>        
        /// <returns></returns>
        public async Task<List<Unit>> GetUserUnitsByIdAsync(long unserId)
        {
            try
            {
                return await _userRepo.GetUserUnitsByIdAsync(unserId).ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
