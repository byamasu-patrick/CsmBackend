using Chat.API.Services;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
//{
//    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
//}));
builder.Services.AddCors(options => {
    options.AddPolicy("corsapp", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((hosts) => true));
});

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corsapp");

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints => {

    endpoints.MapControllers();

    endpoints.MapHub<ChatHub>("/hubs/chat");

});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

//app.UseRouting();


//app.UseAuthorization();

//app.MapControllers();

//app.MapHub<ChatHub>("/hubs/chat");


//app.Run();
