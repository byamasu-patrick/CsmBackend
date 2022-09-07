using AmpMailer.Contracts;
using AmpMailer.Services;
using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpMailer.EventBusConsumer
{
    public class SendingEmailEventConsumer : IConsumer<EmailEvent>
    {
        private readonly ILogger<SendingEmailEventConsumer> _logger;
        private readonly IMapper _mapper;
        private readonly IEmailService _mailService;

        public SendingEmailEventConsumer(ILogger<SendingEmailEventConsumer> logger, IMapper mapper, IEmailService emailService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task Consume(ConsumeContext<EmailEvent> context)
        {
            var data = _mapper.Map<EmailEventData>(context.Message);

            _mailService.SendEmail(data);

            _logger.LogInformation("Email Sending consumed successfully.");
        }
    }
}
