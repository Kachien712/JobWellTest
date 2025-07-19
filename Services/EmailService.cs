using JobWell.Models;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

namespace JobWell.Services
{
    public class EmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                client.Connect(_mailSettings.SmtpServer, _mailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate(_mailSettings.SenderEmail, _mailSettings.Password);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
