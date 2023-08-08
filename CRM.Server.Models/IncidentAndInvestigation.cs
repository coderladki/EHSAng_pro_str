namespace EHS.Server.Models
{
    public class TypeOfIncident
    {
        public int TypeOfIncidentId { get; set; }
        public string IncidentName { get; set; }
        public string IncidentDisplayName { get; set; }
        public int ParentId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public string Remarks { get; set; }
        public string HelpText { get; set; }
    }
    public class Categories
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
    }
    public class IncidentActivity
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string ActivityDisplayName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
    }
    public class Bodypart
    {
        public int BodyPartId { get; set; }
        public string BodyPartName { get; set; }
        public string BodyPartDisplayName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
    }
    public class PPE
    {
        public int PPEId { get; set; }
        public string PPEName { get; set; }
        public string PPEDisplayName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
    }
}
