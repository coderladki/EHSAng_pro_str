using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Server.Models.Dashboards
{
    public class DashboardMenu
    {
        public int Id { get; set; }
        public int MenuOrder { get; set; }
        public string Title { get; set; }
        public string Parent { get; set; }
        public string Level { get; set; }
        public string Hierarchy { get; set; }
        public string Actualpath { get; set; }
    }
}
