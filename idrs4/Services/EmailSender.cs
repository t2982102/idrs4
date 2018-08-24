using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace idrs4.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public IConfiguration _configuration;
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            string myemail = _configuration.GetSection("EmailConfig")["MyEmail"];
            string host = _configuration.GetSection("EmailConfig")["host"];
            int port =Convert.ToInt16( _configuration.GetSection("EmailConfig")["port"]);
            string userName = _configuration.GetSection("EmailConfig")["userName"];
            string password = _configuration.GetSection("EmailConfig")["password"];
            var mailMessage = new MailMessage(myemail, email, subject, message);
            mailMessage.IsBodyHtml = true;
            SmtpClient client = new SmtpClient();
            client.Host = host;
            client.Port = port;
            client.Credentials = new NetworkCredential(userName, password);

            client.SendAsync(mailMessage, null);
            return Task.CompletedTask;
        }
    }
}
