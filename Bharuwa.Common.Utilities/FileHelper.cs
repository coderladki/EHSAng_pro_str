using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System;
using Microsoft.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;

namespace Bharuwa.Common.Utilities
{
    public static class FileHelper
    {
        private static Encoding GetEncoding(MultipartSection section)
        {
            var hasMediaTypeHeader =
                MediaTypeHeaderValue.TryParse(section.ContentType, out var mediaType);

            // UTF-7 is insecure and shouldn't be honored. UTF-8 succeeds in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF8.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }

            return mediaType.Encoding;
        }

        public static string GetUniqueQualifiedFileName(string fileName, string filePath)
        {
            var fileExtension = Path.GetExtension(fileName);
            fileName = Path.GetFileNameWithoutExtension(fileName);
            string filename = String.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}__{1}__{2}{3}", DateTime.Now, fileName,
                //Path.GetRandomFileName()
                Guid.NewGuid().ToString().Replace("-", "")
                , fileExtension); ;
            string path = GetNextFileName(Path.Combine(filePath, filename));

            return Path.GetFileName(path);
        }

        public static string GetNextFileName(string fileName)
        {
            string extension = Path.GetExtension(fileName);

            int i = 0;
            while (System.IO.File.Exists(fileName))
            {
                if (i == 0)
                    fileName = fileName.Replace(extension, "(" + ++i + ")" + extension);
                else
                    fileName = fileName.Replace("(" + i + ")" + extension, "(" + ++i + ")" + extension);
            }

            return fileName;
        }

        public static string GetMimeTypeForFileExtension(string filePath)
        {
            const string defaultContentType = "application/octet-stream";

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(filePath, out string contentType))
            {
                contentType = defaultContentType;
            }

            return contentType;
        }
    }
}
