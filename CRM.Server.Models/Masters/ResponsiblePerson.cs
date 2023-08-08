using DocumentFormat.OpenXml.Bibliography;
using System;

namespace EHS.Server.Models.Masters
{
    public class ResponsiblePerson
    {
        public int ResponsibleId { get; set; }
        public string ResponsibleName { get; set; }
        public int DepartmentId { get; set; }
        public int UserId { get; set; }

        public bool IsActive { get; set; }

        public string Remarks { get; set; }

        public int CreatedBy { get; set; }

        //public DateTime? CreateDateUTC { get; set; }
        public int ModifyBy { get; set; }

        //public DateTime? ModifyDateUTC { get; set; }

    }
}
