using System;

namespace EHS.Server.Models.Masters
{
    public class Hazards
    {
         public int HazardId { get; set; }
		public string HazardName { get; set; }
		public bool AskNotes { get; set; }
        public DateTime? CreatedUTCDate { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedUTCDate { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
    }
}
