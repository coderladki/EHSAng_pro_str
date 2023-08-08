using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;

namespace CRM.Server.Web.Api.Filters
{
    public static class GlobalPreferences
    {
        public static IList<string> PermissionDefinition { get; set; }
        public static IList<string> AspNetUserRoles { get; set; }
    }
}
