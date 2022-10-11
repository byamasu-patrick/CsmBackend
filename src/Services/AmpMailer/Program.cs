
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AmpMailer.Services;
using Serilog;
using MassTransit;
using AmpMailer.EventBusConsumer;
using EventBus.Messages.Common;
using Microsoft.Extensions.Hosting;
using MailKit;

IConfiguration configuration;
configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
    .AddJsonFile("appsettings.json")
    .Build();

// Setting up the logger to read the configuration from the configuration builder
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("mailing_service.log")
    .CreateLogger();


await Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLogging(configure => configure.AddSerilog());

        // Adding Dependency Injection
        services.AddSingleton<IConfiguration>(configuration);

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddMassTransit(config => {

            config.AddConsumer<SendingEmailEventConsumer>();

            config.UsingRabbitMq((context, busFactConfig) =>
            {

                var serviceProvider = services.BuildServiceProvider();
                var configurationObject = serviceProvider.GetService<IConfiguration>();

                Console.WriteLine($"Getting RabbitMQ Credentials: {configurationObject["EventBusSettings:HostAddress"]}");

                busFactConfig.Host(configurationObject["EventBusSettings:HostAddress"]);
                busFactConfig.ReceiveEndpoint(EventBusConstants.EmailQueue, c =>
                {
                    c.ConfigureConsumer<SendingEmailEventConsumer>(context);
                });
            });
        });

        services.AddSingleton<IEmailService, EmailService>();
    })
    .Build()
    .RunAsync();