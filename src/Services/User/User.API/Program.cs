using EventBus.Messages.Common;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using User.API.Extensions;
using User.Application;
using User.Infrastructure;
using User.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// MassTransit-RabbitMQ Configuration
// Add MassTransit

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((context, busFactConfig) =>
    {
        busFactConfig.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//app.MigrateDatabase<UserContext>((context, services) =>
//{
//    var logger = services.GetRequiredService<ILogger<UserContextSeed>>();
//    UserContextSeed.SeedAsync(context, logger)
//    .Wait();
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corsapp");

app.UseAuthorization();

app.MapControllers();

app.Run();
