using Microsoft.AspNetCore.Identity;
using System;

namespace CRM.Server.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        public DateTime CreatedDateTimeUtc { get; set; }
            
        public DateTime UpdatedDateTimeUtc { get; set; }
    }
}
