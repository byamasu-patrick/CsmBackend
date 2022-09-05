using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Enums;
using User.Domain.Entities;

namespace User.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<EmailEventStructure> SendActivationMail(string to, string name, string activationToken, string authCallback);
        Task<EmailEventStructure> SendForgotPasswordMail(string to, string name, string forgotPasswordToken, string authCallback);
        Task<EmailTemplate> GetTemplateByType(EmailType emailType);
    }

    public class EmailEventStructure
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
    }
}
