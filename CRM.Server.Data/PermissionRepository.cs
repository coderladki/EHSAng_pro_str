using CRM.Server.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace CRM.Server.data
{
    public class PermissionRepository
    {
        private readonly string _connectionString;
        public PermissionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task MergePermission(Permission permission,CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync(cancellationToken).ConfigureAwait(false); ;                
                await con.ExecuteAsync("sp_MergePermissionDefintion", permission,commandType: System.Data.CommandType.StoredProcedure,commandTimeout:300);
            }
        }
        public async Task<IEnumerable<Permission>> GetAll(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync(cancellationToken).ConfigureAwait(false);
                var result = await con.QueryAsync<Permission>("select * from PermissionDefinition").ConfigureAwait(false);
                return result;
            }
        }

        public async Task<IEnumerable<Permission>> GetAllNavigation(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync(cancellationToken).ConfigureAwait(false);
                var result = await con.QueryAsync<Permission>("select NavMenuId as crmmenuid,KeyValue Name, 1 Status, 'Navigation Permission' Type from  Navigation_Menus where SelfPointerId is null").ConfigureAwait(false);
                return result;
            }
        }
    }
}