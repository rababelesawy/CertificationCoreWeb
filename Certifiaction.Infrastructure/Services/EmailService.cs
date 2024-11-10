using Certification.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Certifiaction.Infrastructure.Services
{
    public class EmailService : IEmailService
    {


        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;

        public EmailService(IConfiguration configuration)
        {
            _smtpClient = new SmtpClient
            {
                Host = configuration["Smtp:Host"],  // e.g. smtp.gmail.com
                Port = int.Parse(configuration["Smtp:Port"]),  // e.g. 587
                EnableSsl = true,
                Credentials = new NetworkCredential(configuration["Smtp:Username"], configuration["Smtp:Password"])
            };

            _fromEmail = configuration["Smtp:FromEmail"];
        }
        public  async Task  SendEmailAsync(string toEmail, string subject, string body)
        {
            var mailMessage = new MailMessage(_fromEmail, toEmail, subject, body)
            {
                IsBodyHtml = true
            };

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}



