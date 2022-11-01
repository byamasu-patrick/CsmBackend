using Chats.API.Models;
using Chats.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

builder.Services.AddCors(options => {
    options.AddPolicy("corsapp", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();

    });
});

builder.Services.AddSignalR();

builder.Services.AddSingleton<IDictionary<string, UserConnection>>( opts =>
{
    return new Dictionary<string, UserConnection>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
 
app.UseCors("corsapp");

app.UseEndpoints(endpoints => {

    endpoints.MapControllers();

    endpoints.MapHub<ChatHub>("/chat");

});

app.MapControllers();

app.Run();
