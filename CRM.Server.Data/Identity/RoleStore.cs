using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using CRM.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using CRM.Server.Models.Configuration;
using System.Security.Claims;

namespace CRM.Server.Data
{
    public class RoleStore : IQueryableRoleStore<ApplicationRole>, IRoleClaimStore<ApplicationRole>
    {
        private readonly string _connectionString;

        IQueryable<ApplicationRole> IQueryableRoleStore<ApplicationRole>.Roles => GetAllRolesAsync();

        public RoleStore(IConfiguration configuration, AppSettings appSettings)
        {
            string usersDBConnectionString = "server=" + appSettings.Server + "; user id=" + appSettings.UserID + "; password=" + appSettings.Password + ";initial catalog=" + appSettings.CentralDatabase + ";connection timeout=600;";
            _connectionString = usersDBConnectionString;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                if (Convert.ToBoolean(await connection.ExecuteScalarAsync<int>($@"IF EXISTS(select name from AspNetRoles 
                    where LOWER(name) in ('superadmin'))
                    begin select 1 IsExists;end else begin select 0 IsExists;end")))
                {
                    throw new Exception("This role name pre defined by system , try another role name");
                }
                if (Convert.ToBoolean(await connection.ExecuteScalarAsync<int>($@"IF EXISTS(select name from AspNetRoles 
                    where LOWER(name) = '{role.Name}')
                    begin select 1 IsExists;end else begin select 0 IsExists;end")))
                {
                    throw new Exception("Role is already Exists");
                }
                role.Id = await connection.QuerySingleAsync<int>($@"INSERT INTO [AspNetRoles] ([Name], [NormalizedName], [CreatedDateTimeUtc], [UpdatedDateTimeUtc])
                    VALUES (@{nameof(ApplicationRole.Name)}, @{nameof(ApplicationRole.NormalizedName)},  @{nameof(ApplicationRole.CreatedDateTimeUtc)},  @{nameof(ApplicationRole.UpdatedDateTimeUtc)});
                    SELECT CAST(SCOPE_IDENTITY() as int)", role).ConfigureAwait(false);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                if (Convert.ToBoolean(await connection.ExecuteScalarAsync<int>($@"IF EXISTS(select name from AspNetRoles 
                    where LOWER(name) in ('superadmin'))
                    begin select 1 IsExists;end else begin select 0 IsExists;end")))
                {
                    throw new Exception("This role name pre defined by system , try another role name");
                }
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($@"UPDATE [AspNetRoles] SET
                    [Name] = @{nameof(ApplicationRole.Name)},
                    [NormalizedName] = @{nameof(ApplicationRole.NormalizedName)},
                    [UpdatedDateTimeUtc] = @{nameof(ApplicationRole.UpdatedDateTimeUtc)}
                    WHERE [Id] = @{nameof(ApplicationRole.Id)}", role).ConfigureAwait(false);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                if (Convert.ToBoolean(await connection.ExecuteScalarAsync<int>($@"IF EXISTS(select name from AspNetRoles 
                    where LOWER(name) in ('superadmin'))
                    begin select 1 IsExists;end else begin select 0 IsExists;end")))
                {
                    throw new Exception("This role name pre defined by system , try another role name");
                }
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
                await connection.ExecuteAsync($"DELETE FROM [AspNetRoles] WHERE [Id] = @{nameof(ApplicationRole.Id)}", role).ConfigureAwait(false);
            }

            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.FromResult(0);
        }

        public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        public async Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
                return await connection.QuerySingleOrDefaultAsync<ApplicationRole>($@"SELECT * FROM [AspNetRoles]
                    WHERE [Id] = @{nameof(roleId)}", new { roleId }).ConfigureAwait(false);
            }
        }

        public async Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ApplicationRole>($@"SELECT * FROM [AspNetRoles]
                    WHERE [NormalizedName] = @{nameof(normalizedRoleName)}", new { normalizedRoleName }).ConfigureAwait(false);
            }
        }
        public IQueryable<ApplicationRole> GetAllRolesAsync()
        {
            //cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var roles = connection.Query<ApplicationRole>($@"SELECT * FROM [AspNetRoles]"
                    ).AsQueryable();
                return roles;
            }
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var sql = $@"select * from AspNetRoleClaims where RoleId = @RoleId";
                await con.OpenAsync(cancellationToken);
                var dbclaimlist = await con.QueryAsync<IdentityRoleClaim<int>>(sql, new { roleId = role.Id }).ConfigureAwait(false);
                var claims = new List<Claim>();
                foreach (var claim in dbclaimlist)
                {
                    claims.Add(new Claim(claim.ClaimType, claim.ClaimValue));
                }
                return claims;
            }
        }

        public async Task AddClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var sql = $@"insert into AspNetRoleClaims(RoleId,ClaimType,ClaimValue) values(@RoleId,@ClaimType,@ClaimValue)";
            //           var sql = @$"if not exists(select * from AspNetRoleClaims where RoleId = @RoleId and ClaimType= @ClaimType and ClaimValue = @ClaimValue) Begin
            //end";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync(cancellationToken);
                int affectedRows = await con.ExecuteAsync(sql, new { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value });
            }
        }

        public async Task RemoveClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var sql = "delete from AspNetRoleClaims where RoleId = @RoleId and ClaimType = @ClaimType and ClaimValue = @ClaimValue";
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync(cancellationToken);
                    await con.ExecuteAsync(sql, new { roleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value });
                    await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
