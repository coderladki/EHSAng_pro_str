using EHS.Server.Data;
using EHS.Server.Models.Masters;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Threading;
using System;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace EHS.Server.Services.Domain
{
    public class HazardsService
    {
        private readonly HazardsRepository _hazardsRepository;

        public HazardsService(string connectionString)
        {
            _hazardsRepository = new HazardsRepository(connectionString);
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
            return await _hazardsRepository.CreateHazardAsync(hazards, cancellationToken).ConfigureAwait(false);
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
            return await _hazardsRepository.GetAllHazardsAsync(hazards).ConfigureAwait(false);
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
            return await _hazardsRepository.UpdateHazardAsync(hazards, cancellationToken).ConfigureAwait(false);
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
            return  await _hazardsRepository.DeleteHazardAsync(hazards).ConfigureAwait(false);
        }
        #endregion
    }
}
