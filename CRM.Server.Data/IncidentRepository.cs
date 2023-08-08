using CRM.Server.Models;
using Dapper;
using EHS.Server.Models;
using EHS.Server.Models.Masters;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EHS.Server.Data
{
    public class IncidentRepository
    {
        private readonly string _connectionString;
        private readonly UserManager<ApplicationUser> _userManager;
        public IncidentRepository(string connectionString)
        {
            _connectionString = connectionString;

        }
        public async Task<List<TypeOfIncident>> GetAllTypeOfIncidentAsync()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = await conn.QueryAsync<TypeOfIncident>("SELECT TypeOfIncidentId,IncidentName,IncidentDisplayName,ParentId,IsActive,IsDefault,Remarks,HelpText FROM tbl_TypeOfIncident(NOLOCK)").ConfigureAwait(false);
                return result.ToList();
            }
        }
        public async Task<List<Categories>> GetAllCategoryAsync()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = await conn.QueryAsync<Categories>("SELECT CategoryId,CategoryName,IsDefault,IsActive,Remarks FROM tbl_category(NOLOCK)").ConfigureAwait(false);
                return result.ToList();
            }
        }
        public async Task<List<IncidentActivity>> GetAllIncidentActivityAsync()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = await conn.QueryAsync<IncidentActivity>("SELECT ActivityId,ActivityName,ActivityDisplayName,IsDefault,IsActive,Remarks FROM tbl_IncidentActivity(NOLOCK)").ConfigureAwait(false);
                return result.ToList();
            }
        }
        public async Task<List<Bodypart>> GetAllBodyPartsAsync()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = await conn.QueryAsync<Bodypart>("SELECT BodyPartId,BodyPartName,BodyPartDisplayName,IsDefault,IsActive,Remarks FROM tbl_BodyParts(NOLOCK)").ConfigureAwait(false);
                return result.ToList();
            }
        }
        public async Task<List<PPE>> GetAllPPEAsync()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = await conn.QueryAsync<PPE>("SELECT PPEId,PPEName,PPEDisplayName,IsDefault,IsActive,Remarks FROM TBL_PPE(NOLOCK)").ConfigureAwait(false);
                return result.ToList();
            }
        }
    }
}
