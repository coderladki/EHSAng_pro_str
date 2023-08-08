using CRM.Server.Models;
using CRM.Server.Models.Dashboards;
using Dapper;
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
    public class DashboardRepository
    {
        private readonly string _connectionString;
        public DashboardRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<DashboardMenu>> GetAllDashboardMenusAsync(int id)
        {

            using (var conn = new SqlConnection(_connectionString))
            {
                var result = await conn.QueryAsync<DashboardMenu>($@";WITH  MyCTE
                   AS
                (
                        --anchor
                    SELECT  keyvalue,NavMenuId Id, DisplayName[Title], SelfPointerId Parent, 1 AS[Level],
                                 CAST((Actualpath) AS VARCHAR(MAX)) AS Hierarchy, Actualpath,OrderId
                    FROM    Navigation_Menus t1
                    WHERE   SelfPointerId IS NULL

                    UNION ALL
                        --recursive member
                    SELECT  t2.keyvalue,t2.NavMenuId Id, t2.DisplayName [Title], t2.SelfPointerId Parent, M.[level] + 1 AS[Level],
                                 CAST((M.Hierarchy + '->' + t2.Actualpath) AS VARCHAR(MAX)) AS Hierarchy, t2.Actualpath,t2.OrderId
                    FROM    Navigation_Menus AS t2
                            JOIN MyCTE AS M ON t2.SelfPointerId = M.Id
                )

                SELECT * FROM MyCTE  where keyvalue in (select ClaimValue from AspNetUserClaims where userid={id}) order by OrderId").ConfigureAwait(false);
                return result.ToList();
            }
        }
    
    }
}
