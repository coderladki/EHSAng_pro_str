using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Threading;
using EHS.Server.Models.Masters;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System;

namespace EHS.Server.Data
{
    /// <summary>
    /// Author: Ajay Singh
    /// Created Date: 09/03/2023
    /// Contains all the operations related to Hazards
    /// </summary>
    public class HazardsRepository
    {
        private readonly string _connectionString;
        public HazardsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        #region CRUD
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 09/03/2023
        /// To create a hazards 
        /// </summary>
        /// <param name="hazards"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Hazards> CreateHazardAsync(Hazards hazards, CancellationToken cancellationToken)
        {
            try
            {
                string commandName = "usp_HazardsMaster_GetAll";
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_connectionString))
                {                   
                    await connection.OpenAsync(cancellationToken);
                    hazards.HazardId = await connection.ExecuteScalarAsync<int>(commandName, commandType: System.Data.CommandType.StoredProcedure, param: hazards);
                }
                return hazards;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 09/03/2023
        /// To get all Hazards  
        /// </summary>
        /// <param name="hazards"></param>
        /// <returns></returns>
        public async Task<List<Hazards>> GetAllHazardsAsync(Hazards hazards)
        {
            try
            {
                string commandName = "usp_HazardsMaster_GetAll";
                using (var conn = new SqlConnection(_connectionString))
                {
                    var result = await conn.QueryAsync<Hazards>(commandName, hazards.HazardId).ConfigureAwait(false);
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
        /// Created Date: 09/03/2023
        /// To update a Hazards by id  
        /// </summary>
        /// <param name="division"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Hazards> UpdateHazardAsync(Hazards hazards, CancellationToken cancellationToken)
        {
           
            try
            {                
                using (var connection = new SqlConnection(_connectionString))
                {
                    string CommamdText = "usp_EmailMasters_AddUpdate";
                    cancellationToken.ThrowIfCancellationRequested();
                    await connection.OpenAsync(cancellationToken);
                    hazards.HazardId = await connection.ExecuteScalarAsync<int>(CommamdText, commandType: System.Data.CommandType.StoredProcedure, param: hazards);
                    return hazards;
                }
            }
            catch (Exception)
            {
                throw;

            }          
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 09/03/2023
        /// To update a Hazards by id   
        /// </summary>
        /// <param name="hazards"></param>
        /// <returns></returns>
        public async Task<bool> DeleteHazardAsync(Hazards hazards)
        {

            string CommamdText = "usp_HazardsMaster_Delete";
            var rowswsEffected = 0;
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);
                     rowswsEffected= await connection.ExecuteAsync(CommamdText, commandType: System.Data.CommandType.StoredProcedure, param: new
                    {
                        EmailMasterId = hazards.HazardId,
                        ModifiedBy = hazards.ModifiedBy
                    }).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return rowswsEffected > 0?true:false;
        }
        #endregion
    }
}
