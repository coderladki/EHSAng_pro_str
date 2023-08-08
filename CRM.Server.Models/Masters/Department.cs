using System;
using System.ComponentModel.DataAnnotations;

namespace EHS.Server.Models.Masters
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Required]
        public string DepartmentName { get; set; }
        public int PlantId { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
       // public DateTime? CreatedDateUTC { get; set; }
        public int ModifyBy { get; set; }
       // public DateTime? ModifyDateUTC { get; set; }
    }
}
