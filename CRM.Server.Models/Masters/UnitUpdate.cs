using Microsoft.AspNetCore.Http;

namespace EHS.Server.Models.Masters
{
    public class UnitUpdate
    {
        public int UnitId { get; set; }
        public string UnitCode { get; set; }
        public string UnitDisplayName { get; set; }
        public int DivisionId { get; set; }
        public string UnitUserName { get; set; }

        public string UnitUserPassword { get; set; }

        public long UnitEHSHead { get; set; }
        public long UnitEHSAdmin { get; set; }
        public long UnitHead { get; set; }
        public bool UnitStatus { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
        public string Modoules { get; set; }
        public IFormFile? MobileFile { get; set; }
        public IFormFile? WebFile { get; set; }
    }
}
