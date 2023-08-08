using CRM.Server.Data;
using CRM.Server.Models.Dashboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Server.Services.Domain
{
    public class DashboardService
    {

        private readonly DashboardRepository _DashboardMenuRepository;
        public DashboardService(string connectionString)
        {
            _DashboardMenuRepository = new DashboardRepository(connectionString);
        }

        public async Task<List<DashboardMenu>> GetAllDashboardMenusAsync(int id)
        {
            return await _DashboardMenuRepository.GetAllDashboardMenusAsync(id).ConfigureAwait(false);
        }
        //public async Task<List<Lead>> DashboardLeadList()
        //{
        //    return await _DashboardMenuRepository.DashboardLeadList().ConfigureAwait(false);
        //}
        //public async Task<IEnumerable<LeadSummary>> DashboardLeadSummary(int userid)
        //{
        //    return await _DashboardMenuRepository.DashboardLeadSummary(userid).ConfigureAwait(false);
        //}
        //public async Task<List<Schedule>> DashboardScheduleList()
        //{
        //    return await _DashboardMenuRepository.DashboardScheduleList().ConfigureAwait(false);
        //}
    }
}

