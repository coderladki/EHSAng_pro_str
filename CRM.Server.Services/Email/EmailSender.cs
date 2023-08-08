using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CRM.Server.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message, string filePath, bool isBodyHtml = false)
        {
            try
            {
                string senderaddress = "no.reply@patanjaliayurved.org";
                string senderpassword = "Patanjali#07";
                string smtpSender = "smtp.rediffmailpro.com";
                MailMessage msz = new MailMessage();

                msz.From = new MailAddress("no-reply@test.test");
                msz.To.Add(new MailAddress(email));// Change this where you want to receice the mail
                msz.Subject = subject;
                msz.Body = message;
                msz.IsBodyHtml = isBodyHtml;
                System.Net.Mail.Attachment attachment;

                if (!string.IsNullOrEmpty(filePath))
                {
                    attachment = new System.Net.Mail.Attachment(filePath);
                    msz.Attachments.Add(attachment);
                }

                using (System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient(smtpSender, 587))
                {
                    SmtpServer.UseDefaultCredentials = false; //Need to overwrite this
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(senderaddress, senderpassword);//senderaddress and password
                    SmtpServer.Send(msz);
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public Task SendEmailAsync2(string email, string senderAddress, string senderPassword, string subject, string message, string filepath, bool isBodyHtml = false)
        {
            try
            {
                MailMessage msz = new MailMessage();

                msz.From = new MailAddress(senderAddress);
                msz.To.Add(new MailAddress(email));// Change this where you want to receice the mail
                msz.Subject = subject;
                msz.Body = message;
                msz.IsBodyHtml = isBodyHtml;
                System.Net.Mail.Attachment attachment;

                if (!string.IsNullOrEmpty(filepath))
                {
                    attachment = new System.Net.Mail.Attachment(filepath);
                    msz.Attachments.Add(attachment);
                }
                using (System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient("smtp.rediffmailpro.com", 587))
                {
                    SmtpServer.UseDefaultCredentials = false; //Need to overwrite this
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(senderAddress, senderPassword);//senderaddress and password
                    SmtpServer.Send(msz);
                    //delete the file after sending mail
                    //deletefile(filepath);
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            } 
        }

        public Task SendEmailAsync3(string email, string senderAddress, string senderPassword, string subject, string message, string filepath, bool isBodyHtml = false)
        {
            try
            {
                MailMessage msz = new MailMessage();

                msz.From = new MailAddress(senderAddress);
                msz.To.Add(new MailAddress(email));// Change this where you want to receice the mail
                msz.Subject = subject;
                msz.Body = message;
                msz.IsBodyHtml = isBodyHtml;
                System.Net.Mail.Attachment attachment;

                if (!string.IsNullOrEmpty(filepath))
                {
                    attachment = new System.Net.Mail.Attachment(filepath);
                    msz.Attachments.Add(attachment);
                }
                using (System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient("smtp.rediffmailpro.com", 587))
                {
                    SmtpServer.UseDefaultCredentials = false; //Need to overwrite this
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(senderAddress, senderPassword);//senderaddress and password
                    SmtpServer.Send(msz);
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);

            }

        }
    }
}
