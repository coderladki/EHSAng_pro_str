using System;
using System.ComponentModel.DataAnnotations;

namespace EHS.Server.Web.Api.DataObjects.NewFolder
{
    public class DivisionDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDateTimeUtc { get; set; }
    }

    public class EmployeeDto
    {
        public int EmployeeId { get; set; }

        public DateTime DateofBirth { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Department { get; set; }

        public string Designation { get; set; }

        public string Unit { get; set; }

        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Other { get; set; }
        public string Agency { get; set; }

        public int UserId { get; set; }
        public int Gender { get; set; }
        public Boolean XEmployee { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDateTimeUtc { get; set; }

    }
}
