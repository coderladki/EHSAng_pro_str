using CRM.Server.Models.Configuration;
using CRM.Server.Services.Logger;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace CRM.Server.Services
{
    public static class GlobalStaticClass
    { 
        public static void InitiallizeObjects(AppSettings appSettings)
        {
            AppSettings = appSettings;
        }
        public static AppSettings AppSettings { get; private set; }
        public static void SaveAuditLogInFile(string message, LogType type = LogType.Exception)
        {
            if (GlobalStaticClass.AppSettings.LogToFile == true)
            {
                FileLogger.Instance.LogToFile(message, type);
            }
        }
        public static void generatePdf(string htmlfilename)
        {
            string htmlfilePath = @"../wwwroot/Assets/QuotationMail/quotationMail.html";
            var Renderer = new IronPdf.ChromePdfRenderer();
            using var PDF = Renderer.RenderHTMLFileAsPdf(htmlfilename);
            // HTML assets such as images, css and JS will be automatically loaded. 
            PDF.SaveAs("Quotation.pdf"); // oruse PDF.Stream
        }
        public static void SendEmail(string receiverAddress, string senderaddress, string senderpassword, string mailSubject, string body, string filepath, bool isbodyHtml = false)
        {
            try
            {
                string smtpSender = "";

                //getSenderMailPasswod(constr, out senderaddress, out senderpassword);
                if (string.IsNullOrEmpty(senderaddress)) {
                    senderaddress = "no.reply@patanjaliayurved.org";
                    senderpassword = "Patanjali#07";
                    smtpSender = "smtp.rediffmailpro.com";

                }
                else
                {
                    smtpSender = "smtp.bharuwasolution.com";
                }             
                int smtpport = 587;

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(senderaddress);//senderaddress
                    mail.To.Add(receiverAddress);
                    mail.Subject = mailSubject;
                    mail.Body = body;
                    mail.IsBodyHtml = isbodyHtml;
                    System.Net.Mail.Attachment attachment;
                    if (!string.IsNullOrEmpty(filepath))
                    {
                        attachment = new System.Net.Mail.Attachment(filepath);
                        mail.Attachments.Add(attachment);
                    }
                    using (System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient(smtpSender, smtpport))
                    {
                        SmtpServer.UseDefaultCredentials = false; //Need to overwrite this
                        SmtpServer.EnableSsl = true;
                        SmtpServer.Credentials = new System.Net.NetworkCredential(senderaddress, senderpassword);//senderaddress and password
                        SmtpServer.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
