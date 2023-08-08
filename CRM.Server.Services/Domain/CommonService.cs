using CRM.Server.Data;
using CRM.Server.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Server.Services.Domain
{
    public class CommonService
    {
        private readonly CommonRepository _commonRepository;

        public CommonService(string connectionString)
        {
            _commonRepository = new CommonRepository(connectionString);
        }

        public async Task<List<State_Master>> GetAllStateAsync()
        {
            return await _commonRepository.GetAllStateAsync().ConfigureAwait(false);
        }

        public async Task<List<Districts>> GetAllDistrictsAsync(string StateName)
        {
            return await _commonRepository.GetAllDistrictsAsync(StateName).ConfigureAwait(false);
        }
    }
}