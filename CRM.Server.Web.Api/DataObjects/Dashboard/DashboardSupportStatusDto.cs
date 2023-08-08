using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Server.Web.Api.DataObjects.Dashboard
{
    public class DashboardSupportStatusDto
    {

        public string Total { get; set; }
        public string Solved { get; set; }

        public string Pending { get; set; }
    }
}
