using CRM.Server.Models.Common;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Server.Data
{
    public class CommonRepository
    {
        private readonly string _connectionString;

        public CommonRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<State_Master>> GetAllStateAsync()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = await conn.QueryAsync<State_Master>("select * from [dbo].[state_master] order by StateName asc  ").ConfigureAwait(false);
                return result.ToList();
            }
        }

        public async Task<List<Districts>> GetAllDistrictsAsync(string StateName)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = await conn.QueryAsync<Districts>("select * from [dbo].[districts] where State ='" + StateName + "'",conn).ConfigureAwait(false);
                return result.ToList();
            }
        }

    }
}