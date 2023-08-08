using System;
using System.IO;

namespace CRM.Server.Web.Api.Core.Helpers
{
    public class MailContent
    {
        public string ReadQuotationMail()
        {
            try
            {
                var d = Directory.GetCurrentDirectory();
                string directoryPath = null;
                directoryPath = Path.Combine(d, "wwwroot", "Assets", "QuotationMail");

                string filePath = Path.Combine(directoryPath, "QuotationMail.html");
                string body = "";
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    body = reader.ReadToEnd();
                }
                return body;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
