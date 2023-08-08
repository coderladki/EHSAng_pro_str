using System;

namespace EHS.Server.Models.Masters
{
    /// <summary>
    /// Author: Ajay Singh
    /// Created Date: 02/03/2023
    /// To get all unit details
    /// </summary>
    public class UnitDetails
    {
        public int UnitId { get; set; }
        public string UnitCode { get; set; }
        public string UnitDisplayName { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public string UnitUserName { get; set; }
        public string UnitUserPassword { get; set; }

        public long UnitEHSHead { get; set; }
        public string UnitEHSHeadUser { get; set; }
        public string UnitEHSHeadPhone { get; set; }
        public long UnitEHSAdmin { get; set; }
        public string UnitEHSAdminUser { get; set; }
        public string UnitEHSAdminPhone { get; set; }
       
        public long UnitHead { get; set; }
        public string UnitHeadUser { get; set; }
        public string UnitHeadPhone { get; set; }
        public bool UnitStatus { get; set; }
        public DateTime? CreatedUTCDate { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime? ModifiedUTCDate { get; set; }
        public string Modules { get; set; }
        public string MobileFile { get; set; }
        public string WebFile { get; set; }
        
    }
}
