using Discount.Grpc.Extensions;
using Discount.Grpc.Repositories;
using Discount.Grpc.Repositories.Interfaces;
using Discount.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
// Create a host builder to migrate the database at the startup
builder.Host.ConfigureServices(services =>
{
    services.AddSingleton<IHostExtensions, HostExtensions>();
});

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

app.Services.GetRequiredService<IHostExtensions>();

// Configure the HTTP request pipeline.
 app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
