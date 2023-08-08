using System;
using System.Collections.Generic;

namespace CRM.Server.Web.Api.User.DataObjects
{
    public class UserResponseDto
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Picture { get; set; }

        public int Status { get; set; }

        public List<string> Roles { get; set; }

        public string Role {
            get
            { 
                if (Roles != null && Roles.Count > 0)
                {
                    return Roles[0];
                }
                return "";
            } 
        }

        public DateTime CreatedDateTimeUtc { get; set; }

        public DateTime UpdatedDateTimeUtc { get; set; }


        public class RollResponseDto
        {
            public long Id { get; set; }

            public string Name { get; set; }

            public string NormalizedName { get; set; }

            public string ConcurrencyStamp { get; set; }

            public DateTime CreatedDateTimeUtc { get; set; }

            public DateTime UpdatedDateTimeUtc { get; set; }
        }
    }
}
