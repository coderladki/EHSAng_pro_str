using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using CRM.Server.Data.Identity;
using CRM.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;   
using CRM.Server.Models.Configuration;
using System.Security.Claims;

namespace CRM.Server.Data
{
    public class UserStore : IQueryableUserStore<ApplicationUser>, IUserStore<ApplicationUser>, IUserEmailStore<ApplicationUser>, IUserPhoneNumberStore<ApplicationUser>,
        IUserTwoFactorStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserRoleStore<ApplicationUser>, IUserClaimStore<ApplicationUser>
    //, IApplicationUserStore<ApplicationUser>
    {
        private readonly string _connectionString;

        IQueryable<ApplicationUser> IQueryableUserStore<ApplicationUser>.Users => GetAllApplicationUser();

        public UserStore(IConfiguration configuration, AppSettings appSettings)
        {
            string usersDBConnectionString = "server=" + appSettings.Server + "; user id=" + appSettings.UserID + "; password=" + appSettings.Password + ";initial catalog=" + appSettings.CentralDatabase + ";connection timeout=600;";
            _connectionString = usersDBConnectionString;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                if(user.PlantId==0 || user.PlantId==null)
                {
                   
                    user.PlantId = await connection.QuerySingleAsync<int>($@"SELECT ISNULL((SELECT TOP 1 UnitId FROM tbl_UnitMaster WHERE UnitDisplayName='{user.PlantName}' OR UnitUserName='{user.PlantName}'),0)");
                }

                user.Id = await connection.QuerySingleAsync<int>($@"INSERT INTO [AspNetUsers] ([UserName], [NormalizedUserName], [Email],
                    [NormalizedEmail], [EmailConfirmed],[FirstName],[LastName], [PasswordHash], [PlantId], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled],[AccessFailedCount],[Gender], [CreatedDateTimeUtc], [UpdatedDateTimeUtc],[Status],[IsLoginEnabled])
                    VALUES (@{nameof(ApplicationUser.UserName)}, @{nameof(ApplicationUser.NormalizedUserName)}, @{nameof(ApplicationUser.Email)},
                    @{nameof(ApplicationUser.NormalizedEmail)}, @{nameof(ApplicationUser.EmailConfirmed)}, @{nameof(ApplicationUser.FirstName)}, @{nameof(ApplicationUser.LastName)}, @{nameof(ApplicationUser.PasswordHash)}, @{nameof(ApplicationUser.PlantId)},
                    @{nameof(ApplicationUser.PhoneNumber)}, @{nameof(ApplicationUser.PhoneNumberConfirmed)}, @{nameof(ApplicationUser.TwoFactorEnabled)}, @{nameof(ApplicationUser.LockoutEnabled)}, @{nameof(ApplicationUser.AccessFailedCount)}, @{nameof(ApplicationUser.Gender)},  @{nameof(ApplicationUser.CreatedDateTimeUtc)}, @{nameof(ApplicationUser.UpdatedDateTimeUtc)},@{nameof(ApplicationUser.Status)},@{nameof(ApplicationUser.IsLoginEnabled)});
                    SELECT CAST(SCOPE_IDENTITY() as int)", user).ConfigureAwait(false);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
                await connection.ExecuteAsync($"DELETE FROM [AspNetUsers] WHERE [Id] = @{nameof(ApplicationUser.Id)}", user).ConfigureAwait(false);
            }

            return IdentityResult.Success;
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"SELECT * FROM [AspNetUsers]
                    WHERE [Id] = @{nameof(userId)}", new { userId }).ConfigureAwait(false);
            }
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"SELECT [Id] ,[UserName],[NormalizedUserName] ,em.[Email] ,[NormalizedEmail] ,[EmailConfirmed] ,[PasswordHash],[SecurityStamp] ,[ConcurrencyStamp],[PhoneNumber] ,[PhoneNumberConfirmed],[TwoFactorEnabled] ,[LockoutEnd]
      ,[LockoutEnabled] ,[AccessFailedCount],em.[Gender] ,u.[FirstName],u.[LastName] ,u.[CreatedDateTimeUtc],u.[UpdatedDateTimeUtc] ,ISNULL([PlantId],0)[PlantId],[Status],[IsLoginEnabled],em.Designation,em.Department, um.UnitDisplayName PlantName FROM [AspNetUsers](NOLOCK) u  
      LEFT JOIN [tbl_EmployeeMaster](NOLOCK) em on u.Id=em.UserId
      LEFT JOIN [tbl_UnitMaster](NOLOCK) um on u.PlantId=um.UnitId WHERE [NormalizedUserName] = @{nameof(normalizedUserName)}", new { normalizedUserName }).ConfigureAwait(false);
            }
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($@"UPDATE [AspNetUsers] SET
                    [UserName] = @{nameof(ApplicationUser.UserName)},
                    [NormalizedUserName] = @{nameof(ApplicationUser.NormalizedUserName)},
                    [Email] = @{nameof(ApplicationUser.Email)},
                    [FirstName] = @{nameof(ApplicationUser.FirstName)},
                    [LastName] = @{nameof(ApplicationUser.LastName)},
                    [NormalizedEmail] = @{nameof(ApplicationUser.NormalizedEmail)},
                    [EmailConfirmed] = @{nameof(ApplicationUser.EmailConfirmed)},
                    [PasswordHash] = @{nameof(ApplicationUser.PasswordHash)},
                    [PhoneNumber] = @{nameof(ApplicationUser.PhoneNumber)},
                    [PhoneNumberConfirmed] = @{nameof(ApplicationUser.PhoneNumberConfirmed)},
                    [TwoFactorEnabled] = @{nameof(ApplicationUser.TwoFactorEnabled)},
                    [CreatedDateTimeUtc]= @{nameof(ApplicationUser.CreatedDateTimeUtc)},
                    [UpdatedDateTimeUtc]= @{nameof(ApplicationUser.UpdatedDateTimeUtc)},
                    [IsLoginEnabled]=@{nameof(ApplicationUser.IsLoginEnabled)}
                    WHERE [Id] = @{nameof(ApplicationUser.Id)}", user).ConfigureAwait(false);
            }
                
            return IdentityResult.Success;
        }

        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"SELECT * FROM [AspNetUsers]
                    WHERE [NormalizedEmail] = @{nameof(normalizedEmail)}", new { normalizedEmail }).ConfigureAwait(false);
            }
        }

        public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }


        public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public async Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                var normalizedName = roleName.ToUpper();
                var roleId = await connection.ExecuteScalarAsync<int?>($"SELECT [Id] FROM [AspNetRoles] WHERE [NormalizedName] = @{nameof(normalizedName)}", new { normalizedName }).ConfigureAwait(false);
                if (!roleId.HasValue)
                {
                    var role = new ApplicationRole
                    {
                        Name = roleName,
                        NormalizedName = normalizedName,
                        CreatedDateTimeUtc = DateTime.Now,
                        UpdatedDateTimeUtc = DateTime.Now
                    };
                    roleId = await connection.QuerySingleAsync<int>($@"INSERT INTO [AspNetRoles] ([Name], [NormalizedName], [CreatedDateTimeUtc], [UpdatedDateTimeUtc])
                    VALUES (@{nameof(ApplicationRole.Name)}, @{nameof(ApplicationRole.NormalizedName)},  @{nameof(ApplicationRole.CreatedDateTimeUtc)},  @{nameof(ApplicationRole.UpdatedDateTimeUtc)});
                    SELECT CAST(SCOPE_IDENTITY() as int)", role).ConfigureAwait(false);
                }

                await connection.ExecuteAsync($"IF NOT EXISTS(SELECT 1 FROM [AspNetUserRoles] WHERE [UserId] = @userId AND [RoleId] = @{nameof(roleId)}) " +
                    $"INSERT INTO [AspNetUserRoles]([UserId], [RoleId]) VALUES(@userId, @{nameof(roleId)})",
                    new { userId = user.Id, roleId }).ConfigureAwait(false);
                
                await connection.ExecuteAsync($@"insert into AspNetUserClaims(UserId, ClaimType, ClaimValue)
                    select @userId,ClaimType,ClaimValue from AspNetRoleClaims where RoleId = @roleId",
                    new { userId = user.Id, roleId }).ConfigureAwait(false);
            }
        }

        public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                var roleId = await connection.ExecuteScalarAsync<int?>("SELECT [Id] FROM [AspNetRoles] WHERE [NormalizedName] = @normalizedName", new { normalizedName = roleName.ToUpper() }).ConfigureAwait(false);
                if (roleId.HasValue)
                {
                    await connection.ExecuteAsync($"DELETE FROM [AspNetUserRoles] WHERE [UserId] = @userId AND [RoleId] = @{nameof(roleId)}", new { userId = user.Id, roleId }).ConfigureAwait(false);
                    var RolePermissions = await connection.QueryAsync<string>($"select ClaimValue from AspNetRoleClaims where RoleId = @RoleId", new {RoleId = roleId }).ConfigureAwait(false);
                    foreach (var RolePermission in RolePermissions)
                    {
                        var PermissionExist = (await connection.QueryAsync<int>($"select count(*) from AspNetUserClaims where UserId = @UserId and ClaimValue = @ClaimValue and claimType = 'Permission'", new { UserId = user.Id,ClaimValue = RolePermission}).ConfigureAwait(false))?.FirstOrDefault();
                        if (PermissionExist >0)
                        {
                            var rows = (await connection.QueryAsync(@$"select count(*) from AspNetRoleClaims where RoleId in (select RoleId From AspNetUserRoles where UserId = @UserId and RoleId != @RoleId) and 
                                    claimType = 'Permission' and Claimvalue = @ClaimValue", new {UserId = user.Id,ClaimValue = RolePermission,RoleId = roleId }).ConfigureAwait(false)).FirstOrDefault();
                            if (rows ==0)
                            {
                                await connection.QueryAsync<string>($"delete from AspNetUserClaims where UserId = @UserId claimType = 'Permission' and ClaimValue = @ClaimValue", new { UserId = user.Id,ClaimValue = RolePermission}).ConfigureAwait(false);
                            }                            
                        }
                    }                    
                }                    
            }
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
                var queryResults = await connection.QueryAsync<string>("SELECT r.[Name] FROM [AspNetRoles] r INNER JOIN [AspNetUserRoles] ur ON ur.[RoleId] = r.Id " +
                    "WHERE ur.UserId = @userId", new { userId = user.Id }).ConfigureAwait(false);

                return queryResults.ToList();
            }
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                var roleId = await connection.ExecuteScalarAsync<int?>("SELECT [Id] FROM [AspNetRoles] WHERE [NormalizedName] = @normalizedName", new { normalizedName = roleName.ToUpper() }).ConfigureAwait(false);
                if (roleId == default(int)) return false;
                var matchingRoles = await connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM [AspNetUserRoles] WHERE [UserId] = @userId AND [RoleId] = @{nameof(roleId)}",
                    new { userId = user.Id, roleId }).ConfigureAwait(false);

                return matchingRoles > 0;
            }
        }

        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                var queryResults = await connection.QueryAsync<ApplicationUser>("SELECT u.* FROM [AspNetUsers] u " +
                    "INNER JOIN [AspNetUserRoles] ur ON ur.[UserId] = u.[Id] INNER JOIN [AspNetRoles] r ON r.[Id] = ur.[RoleId] WHERE r.[NormalizedName] = @normalizedName",
                    new { normalizedName = roleName.ToUpper() }).ConfigureAwait(false);

                return queryResults.ToList();
            }
        }

        public  IQueryable<ApplicationUser> GetAllApplicationUser()
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                var users =  connection.Query<ApplicationUser>($@"SELECT * FROM [AspNetUsers]");
                return users.AsQueryable(); ;
            }
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }
        
        public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var sql = $@"select * from AspNetUserClaims where UserId = @UserId";
                con.OpenAsync(cancellationToken);
                var dbclaimlist = await con.QueryAsync<IdentityUserClaim<int>>(sql, new { UserId = user.Id }).ConfigureAwait(false);
                var claims = new List<Claim>();
                foreach (var claim in dbclaimlist)
                {   
                    claims.Add(new Claim(claim.ClaimType, claim.ClaimValue));
                }
                return claims;
            }        
        }

        public async Task AddClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var sql = @$"if not exists(select * from AspNetUserClaims where userid = {user.Id} and ClaimType = @Type and ClaimValue = @Value) Begin
insert into AspNetUserClaims(UserId,ClaimType,ClaimValue) values({user.Id},@Type,@Value) End";            
            
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync(cancellationToken);
                var rows = await con.ExecuteAsync(sql,claims);
                
            }
        }

        public Task ReplaceClaimAsync(ApplicationUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var sql = $"delete from AspNetUserClaims where UserId = {user.Id} and ClaimType = @Type and ClaimValue = @Value";           
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync(cancellationToken);
                var rowsAffected =  await con.ExecuteAsync(sql, claims);                
            }
        }        

        public async Task<IList<ApplicationUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();            
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var sql = "select * from AspNetUsers u inner join AspNetUserClaims c on c.UserId = u.Id where c.ClaimType = @Type and c.ClaimValue = @Value";
                await con.OpenAsync(cancellationToken);
               var result = (await con.QueryAsync<ApplicationUser>(sql,claim).ConfigureAwait(false)).ToList();
                return result;
            }

        }
    }
}
