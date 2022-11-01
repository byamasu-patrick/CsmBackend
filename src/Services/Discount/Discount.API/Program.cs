using Discount.API.Repositories;
using Discount.API.Repositories.Interfaces;
using Discount.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Create a host builder to migrate the database at the startup
builder.Host.ConfigureServices(services =>
{
    services.AddSingleton<IHostExtensions, HostExtensions>();
});


// Add services to the container.
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();


app.Services.GetRequiredService<IHostExtensions>();
   

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
