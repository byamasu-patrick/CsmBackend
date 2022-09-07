using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

// Configure logging 

builder.Host.ConfigureLogging((hostingContext, configurationBuilder) =>
{
    configurationBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
    configurationBuilder.AddConsole();
    configurationBuilder.AddDebug();
});

builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);

builder.Services.AddOcelot()
    .AddCacheManager(cacheSettings =>
    {
        cacheSettings.WithDictionaryHandle();
    });

// Configure Ocelot API Gateway to be used in this service

var app = builder.Build();

app.UseOcelot().Wait();

app.Run();
