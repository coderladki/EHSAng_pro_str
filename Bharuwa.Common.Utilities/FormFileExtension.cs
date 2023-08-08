using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bharuwa.Common.Utilities
{
    public static class IFormFileExtension
    {
        private static async Task SaveFileAsync( this IFormFile formFile, string path )
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
        }
    }
}
