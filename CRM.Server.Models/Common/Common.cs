using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Server.Models.Common
{
    public class Common
    {        
    }

    public class Districts
    {
        public string State { get; set; }

        public string District { get; set; }

        public string StateType { get; set; }
    }

    public class State_Master
    {
        public string StateCode { get; set; }

        public string StateName { get; set; }

        public int STATUS { get; set; }

        public DateTime STAMP { get; set; }

        public string CreateBy { get; set; }

        public string UpdatedBy { get; set; }

        public string InputStateCode { get; set; }
    }
}