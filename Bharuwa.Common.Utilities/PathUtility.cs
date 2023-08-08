using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bharuwa.Common.Utilities
{
    public static class PathUtility
    {
        public static string GetPath(string companyId, string folder, string fileName)
        {
            if (!string.IsNullOrEmpty(companyId))
            {
                fileName = companyId + "-" + DateTime.Now.Ticks + "-" + fileName;
            }

            var d = Directory.GetCurrentDirectory();
            var directoryPath = Path.Combine(d, "wwwroot", folder);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var path = Path.Combine(directoryPath, fileName);
            return path;
        }
    }
}
