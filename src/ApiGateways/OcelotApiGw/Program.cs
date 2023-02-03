using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Loging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddOcelot()
                .AddCacheManager(settings => settings.WithDictionaryHandle());

var app = builder.Build();

app.MapGet("/", () => "This is API Gateway");

await app.UseOcelot();

app.Run();
