using CRM.Server.data;
using CRM.Server.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CRM.Server.Services.Domain
{
    public class PermissionsService
    {
        private readonly PermissionRepository _PermissionRepository;
        public PermissionsService(string connectionString)
        {
            _PermissionRepository = new PermissionRepository(connectionString);
        }

        public async Task MergePermission(Permission permission, CancellationToken cancellationToken)
        {
            await _PermissionRepository.MergePermission(permission,cancellationToken).ConfigureAwait(false);
        }
        public async Task<IEnumerable<Permission>> GetAll(CancellationToken cancellationToken)
        {
            return await _PermissionRepository.GetAll(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Permission>> GetAllNavigation(CancellationToken cancellationToken)
        {
            return await _PermissionRepository.GetAllNavigation(cancellationToken).ConfigureAwait(false);
        }
    }
}
