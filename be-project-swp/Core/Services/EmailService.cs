using NETCore.MailKit.Core;
using System.Net.Mail;
using System.Net;

namespace be_project_swp.Core.Services
{
    public static class EmailService
    {
        public static async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string fromMail = "kietuctse161952@fpt.edu.vn";
            string fromPassword = "tuxh vzux xtqa bzjs";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(email));
            message.Body = "<html><body> " + htmlMessage + " </body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);
        }
    }
}
