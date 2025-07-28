using JobWellTest.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace JobWellTest.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmail(string receptor, string subject, string body)
        {
            var email = configuration.GetValue<string>("Email configuration:Email");
            var password = configuration.GetValue<string>("Email configuration:Password");
            var host = configuration.GetValue<string>("Email configuration:Host");
            var port = configuration.GetValue<int>("Email configuration:Port");

            var smtpClient = new SmtpClient(host, port);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;

            smtpClient.Credentials = new NetworkCredential(email, password);

            var message = new MailMessage(email, receptor, subject, body);
            message.IsBodyHtml = true;
            await smtpClient.SendMailAsync(message);

        }
    }
}
