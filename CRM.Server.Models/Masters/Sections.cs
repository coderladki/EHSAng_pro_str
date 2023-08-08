using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using System;

namespace EHS.Server.Models.Masters
{
    public class Sections
    {
        public int DepartmentSectionId { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentSectionName { get; set; }
        public int PlantId { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        //public DateTime? CreatedDateUTC { get; set; }
        public int ModifyBy { get; set; }
        //public DateTime ModifyDateUTC { get; set; }
    }
}
