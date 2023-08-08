using System;

namespace EHS.Server.Models.Masters
{
    /// <summary>
    /// Author: Ajay Singh
    /// Created Date: 27/02/2023
    /// To Add update  email master 
    /// </summary>
    public class EmailsMaster
    {
        public long EmailMasterId { get; set; }
        public int TypeId { get; set; }
        public int PlantId { get; set; }
        public int DivisionId { get; set; }
        public string To_Emails { get; set; }
        public string CC_Emails { get; set; }
        public int Email_Status { get; set; }
        //public DateTime? CreatedUTCDate { get; set; }
        public long CreatedBy { get; set; }
       // public DateTime? ModifiedUTCDate { get; set; }
        public long ModifiedBy { get; set; }
    }
}
