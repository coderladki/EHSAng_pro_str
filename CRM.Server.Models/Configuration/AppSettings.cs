using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Server.Models.Configuration
{
    public class AppSettings
    {
        public string Database { get; set; }

        public string Server { get; set; }

        public string UserID { get; set; }

        public string Password { get; set; }

        public string EncryptedPassword { get; set; }

        public string PassPhrase { get; set; }

        public bool LogToFile { get; set; }

        public bool AllowProductionQC { get; set; }

        public string CentralDatabase { get; set; }
    }
}
