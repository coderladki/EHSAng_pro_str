namespace CRM.Server.Models
{
    public class Permission
    {
        //generated through identity
       // public int Id {get;}
        public string Type { get; set; }
        //Permission Name
        public string Name { get; set; }
        //status permission Active or InActive
        public bool Status { get; set; }

        public int navigationid { get; set; }

        public int crmmenuid { get; set; }

    }
}
