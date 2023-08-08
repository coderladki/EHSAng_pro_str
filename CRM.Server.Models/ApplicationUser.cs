using CRM.Server.Models.Enum;
using Microsoft.AspNetCore.Identity;
using System;

namespace CRM.Server.Models
{
    public class ApplicationUser  : IdentityUser<int>
    {   
        public eGender Gender { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreatedDateTimeUtc { get; set; }

        public DateTime UpdatedDateTimeUtc { get; set; }

        public int Status { get; set; }

       // public string ActualPassword { get; set; }

        public string PhoneNumber { get; set; }

        public int? PlantId { get; set; }
        public string PlantName { get; set; }
        public bool IsLoginEnabled { get; set; } = false;
        public string Designation { get; set; }
        public string Department { get; set; }
    }
}
