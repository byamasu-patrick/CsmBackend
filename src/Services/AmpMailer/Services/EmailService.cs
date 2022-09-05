using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System.Drawing;
using AmpMailer.Contracts;

namespace AmpMailer.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private MimeMessage _mimeMessage;
        private readonly string SenderEmail;
        private readonly string SecurityKey;
        private readonly int _Port;
        private readonly ISmtpClient _smtpClient;
        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException();
            _logger = logger ?? throw new ArgumentNullException();
            SenderEmail = _configuration.GetSection("SenderEmail").Value;
            SecurityKey = _configuration.GetSection("SecurityKey").Value;
            _Port = Int32.Parse(_configuration.GetSection("EmailPort").Value);
            _smtpClient = new SmtpClient();

            if (!_smtpClient.IsConnected)
            {
                _smtpClient.Connect(_configuration["EmailHost"], _Port, false);
                _smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
            }
            if (!_smtpClient.IsAuthenticated)
            {
                _logger.LogInformation("Authenticating to the server...");
                _smtpClient.Authenticate(SenderEmail.Trim(), SecurityKey.Trim());
            }
        }

        public void Initialize(string To, string Subject, DateTime Date, string Content)
        {

            _mimeMessage = new MimeMessage();
            if (!SenderEmail.Equals(null) && !SecurityKey.Equals(null))
            {
                _mimeMessage.From.Add(MailboxAddress.Parse(SenderEmail));
                _mimeMessage.To.Add(MailboxAddress.Parse(To));
                _mimeMessage.Subject = Subject;

                
                _mimeMessage.Body = new TextPart(TextFormat.Html) { Text = Content };
                return;
            }
        }
        

        public void SendEmail(EmailEventData emailEvent)
        {
            try
            {
                Initialize(emailEvent.To, emailEvent.Subject, emailEvent.Date, emailEvent.Content);
                if (_mimeMessage.Body != null)
                {
                   
                    _logger.LogInformation("Sending the email to " + _mimeMessage.To + "...");
                    _smtpClient.Send(_mimeMessage);
                    _smtpClient.Disconnect(true);                

                    _logger.LogInformation("Email has been sent successfully");
                    if (!_smtpClient.IsConnected)
                    {
                        _smtpClient.Connect(_configuration["EmailHost"], _Port, false);
                        _smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                    }
                    if (!_smtpClient.IsAuthenticated)
                    {
                        _logger.LogInformation("Authenticating to the server...");
                        _smtpClient.Authenticate(SenderEmail.Trim(), SecurityKey.Trim());
                    }
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (NullReferenceException error)
            {
                _logger.LogCritical(new Exception(), "An empty cannot read the system email address and security");
                Console.WriteLine("An error has occured whilet sending the email.");
            }
        }

    }
}
