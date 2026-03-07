using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace PD411_Shop.Services
{
    public class EmailService : IEmailSender
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _email;
        private readonly string _password;
        private readonly SmtpClient _smtpClient;
        public EmailService()
        {
            _email = "espgoodlife22@gmail.com";
            _password = "yxzd zjrw mkxg frrs";
            _host = "smtp.gmail.com";
            _port = 587;

            _smtpClient = new SmtpClient(_host, _port);
            _smtpClient.Credentials = new NetworkCredential(_email, _password);
            _smtpClient.EnableSsl = true;                 // enable coding our data for security 
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_email);
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = htmlMessage;
            mailMessage.IsBodyHtml = true;

            await _smtpClient.SendMailAsync(mailMessage);
        
        }
    }
}
