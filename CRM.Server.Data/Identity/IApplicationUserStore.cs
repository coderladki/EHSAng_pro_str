using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Server.Data.Identity
{
    public interface IApplicationUserStore<TUser> : IUserStore<TUser> where TUser : class
    {
        Task<List<TUser>> GetAllApplicationUserAsync();

    }
}
