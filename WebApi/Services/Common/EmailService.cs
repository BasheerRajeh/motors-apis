using SendGrid.Helpers.Mail;
using SendGrid;
using System.Net.Mail;
using System.Net;

namespace WebApi.Services.Common
{
    public class EmailService
    {
        protected readonly IConfiguration _configs;
        protected readonly ILogger<EmailService> _logger;
        protected readonly string _emailTemplateConfirmEmail;

        public EmailService(IConfiguration configs,
             ILogger<EmailService> logger
            )
        {
            _configs = configs;
            _logger = logger;
            _emailTemplateConfirmEmail = "";// File.ReadAllText("EmailTemplates/email-varification.html");
        }

        public Task SendEmail(string to, string toName, string subject, string body)
        {
            var key = _configs.GetValue<string>("Email:SendGridKey");
            var from = _configs.GetValue<string>("Email:From");
            var fromName = _configs.GetValue<string>("Email:FromName");

            if (string.IsNullOrEmpty(key))
            {
                _logger.LogWarning("Email configurations missing exiting from SendEmail function early.");
                _logger.LogDebug(subject);
                _logger.LogDebug(body);
                return Task.CompletedTask;
            }

            //var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            var client = new SendGridClient(key);
            var msg = MailHelper.CreateSingleEmail(
                new EmailAddress(from, fromName),
                new EmailAddress(to, toName),
                subject,
                body,
                body);
            return client.SendEmailAsync(msg);

        }

        public void SendEmail(string body)
        {
            // Set up SMTP client
            SmtpClient client = new SmtpClient(_configs["Email:Host"], int.Parse(_configs["Email:Port"]));
            client.EnableSsl = bool.Parse(_configs["Email:EnableSsl"]);
            client.UseDefaultCredentials = bool.Parse(_configs["Email:DefaultCredentials"]);
            client.Credentials = new NetworkCredential(_configs["Email:NetworkCredential:Email"], _configs["Email:NetworkCredential:Password"]);

            // Create email message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_configs["Email:NetworkCredential:Email"]);
            mailMessage.To.Add(_configs["Email:NetworkCredential:Email"]);

            mailMessage.Body = body;

            client.Send(mailMessage);
        }

        public Task SendEmailConfirmationToken(string email, int userId, string userName, string token)
        {
            var body = _emailTemplateConfirmEmail
                .Replace("[email_token]", token)
                .Replace("[user_id]", userId.ToString());
            return SendEmail(email, userName, $"Welcome {userName}", body);
        }
    }
}
