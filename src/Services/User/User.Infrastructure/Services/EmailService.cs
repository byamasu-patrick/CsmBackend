using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Contracts.Infrastructure;
using User.Application.Contracts.Persistence;
using User.Application.Enums;
using User.Domain.Entities;

namespace User.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailRepository _emailRepository;

        public EmailService(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }

        public async Task<EmailEventStructure> SendActivationMail(string to, string name, string activationToken, string authCallback)
        {
            var paramsDict = new Dictionary<string, string>
            {
                { "full_name", name },
                { "activation_token", activationToken },
                { "auth_callback_path",  authCallback },
                { "app_name",  "Cloud Store Malawi" },
                { "app_link",  "https://www.cloudstoremalawi.com" }
            };

            var template = await GetTemplateByType(EmailType.Activation);

            foreach (var param in paramsDict)
            {
                template.TemplateContent = template.TemplateContent.Replace("{{" + param.Key + "}}", param.Value);
            }
            // Send email to the queue

            return new EmailEventStructure
            {
                To = to,
                Subject = "Cloud Stores Malawi Activation Email",
                Content = template.TemplateContent,
                Date = DateTime.UtcNow
            };
        }

        public async Task<EmailEventStructure> SendForgotPasswordMail(string to, string name, string forgotPasswordToken, string authCallback)
        {
            var paramsDict = new Dictionary<string, string>
            {
                { "full_name", name },
                { "forgot_password_token", forgotPasswordToken },
                { "auth_callback_path",  authCallback },
                { "app_name",  "Cloud Store Malawi" },
                { "app_link",  "https://www.cloudstoremalawi.com" }
            };

            var template = await GetTemplateByType(EmailType.ForgotPassword);

            foreach (var param in paramsDict)
            {
                template.TemplateContent = template.TemplateContent.Replace("{{" + param.Key + "}}", param.Value);
            }
            return new EmailEventStructure
            {
                To = to,
                Subject = "Cloud Stores Malawi Activation Email",
                Content = template.TemplateContent,
                Date = DateTime.UtcNow
            };
        }

        public async Task<EmailTemplate> GetTemplateByType(EmailType emailType)
        {
            var template = await _emailRepository.GetTemplateByType(emailType.ToString());
            
            return template;
        }

        
    }
}
