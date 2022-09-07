using Microsoft.Extensions.DependencyInjection;
using Basket.API.Repositories.Interfaces;
using Basket.API.Repositories;
using Basket.API.GrpcServices;
using Discount.Grpc.Protos;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Redist Connection
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});

// General Configuration

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

// Grpc Connection
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]);
});

builder.Services.AddScoped <DiscountGrpcService> ();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add MassTransit

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((context, busFactConfig) =>
    {
        busFactConfig.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

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
