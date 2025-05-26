using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace BusinessLogicLayer
{
    //Lucija Polak
    public class EmailService
    {
        private readonly string smtpServer;
        private readonly int smtpPort;
        private readonly string senderEmail;
        private readonly string senderPassword;

        public EmailService()
        {
            smtpServer = ConfigurationManager.AppSettings["SMTPServer"];
            smtpPort = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
            senderEmail = ConfigurationManager.AppSettings["SenderEmail"];
            senderPassword = ConfigurationManager.AppSettings["SenderPassword"];
        }

        public async Task SendEmailAsync(List<string> recipients, string subject, string body)
        {
            using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtpClient.EnableSsl = true;

                foreach (var recipient in recipients)
                {
                    var mailMessage = new MailMessage(senderEmail, recipient, subject, body);
                    mailMessage.IsBodyHtml = true;

                    try
                    {
                        await smtpClient.SendMailAsync(mailMessage);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Greška pri slanju e-maila korisniku {recipient}: {ex.Message}");
                    }
                }
            }
        }
    }
}
