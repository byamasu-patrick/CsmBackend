using Microsoft.EntityFrameworkCore;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using Ordering.API.Extensions;
using MassTransit;
using Ordering.API.EventBusConsumer;
using EventBus.Messages.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// MassTransit-RabbitMQ Configuration
builder.Services.AddMassTransit(config => {

    config.AddConsumer<BasketCheckoutConsumer>();

    config.UsingRabbitMq((context, busFactConfig) => {

        busFactConfig.Host(builder.Configuration["EventBusSettings:HostAddress"]);

        busFactConfig.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c => {
            c.ConfigureConsumer<BasketCheckoutConsumer>(context);
        });
    });

});

builder.Services.AddScoped<BasketCheckoutConsumer>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MigrateDatabase<OrderContext>((context, services) =>
{
    var logger = services.GetRequiredService<ILogger<OrderContextSeed>>();
    OrderContextSeed.SeedAsync(context, logger)
    .Wait();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
