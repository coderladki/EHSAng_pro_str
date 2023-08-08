using EHS.Server.Data;
using EHS.Server.Models;
using EHS.Server.Models.Masters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EHS.Server.Services.Domain
{
    public class IncidentService
    {
        private readonly IncidentRepository _IncidentRepository;

        public IncidentService(string connectionString)
        {
            _IncidentRepository = new IncidentRepository(connectionString);
        }
        public async Task<List<TypeOfIncident>> GetAllTypeOfIncidentAsync()
        {
            return await _IncidentRepository.GetAllTypeOfIncidentAsync().ConfigureAwait(false);
        }
        public async Task<List<Categories>> GetAllCategoryAsync()
        {
            return await _IncidentRepository.GetAllCategoryAsync().ConfigureAwait(false);
        }
        public async Task<List<IncidentActivity>> GetAllIncidentActivityAsync()
        {
            return await _IncidentRepository.GetAllIncidentActivityAsync().ConfigureAwait(false);
        }
        public async Task<List<Bodypart>> GetAllBodyPartsAsync()
        {
            return await _IncidentRepository.GetAllBodyPartsAsync().ConfigureAwait(false);
        }
        public async Task<List<PPE>> GetAllPPEAsync()
        {
            return await _IncidentRepository.GetAllPPEAsync().ConfigureAwait(false);
        }
    }
}
