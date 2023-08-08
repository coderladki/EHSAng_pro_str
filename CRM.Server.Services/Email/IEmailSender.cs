using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Server.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, string filePath, bool isBodyHtml = false);
        Task SendEmailAsync2(string email, string senderAddress, string senderPassword, string subject, string message, string filepath, bool isBodyHtml = false);
    }
}
