using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using System.Configuration;
using Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);

//builder.Logging.AddConfiguration(builder.Configuration.GetSection("Loging"));
//builder.Logging.AddConsole();
//builder.Logging.AddDebug();

builder.Services.AddOcelot()
                .AddCacheManager(settings => settings.WithDictionaryHandle());

var app = builder.Build();

app.MapGet("/", () => "This is API Gateway");

await app.UseOcelot();

app.Run();
