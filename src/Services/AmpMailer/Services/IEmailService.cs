using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmpMailer.Contracts;

namespace AmpMailer.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailEventData emailEvent);
        void Initialize(string To, string Subject, DateTime Date, string Content);

    }
}
