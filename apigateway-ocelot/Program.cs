

using apigateway_ocelot;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder (args);

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var ocelotConfiguration = new ConfigurationBuilder().AddJsonFile($"ocelot.{env}.json").Build();


builder.Configuration.AddConfiguration(ocelotConfiguration);

builder.Services.AddLogging(log => log.AddConsole());

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddOcelot().AddCacheManager(settings => settings.WithDictionaryHandle());
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hellow world");
    });
});


app.UseHttpsRedirection();

app.UseMiddleware<ApiKeyAuthHandler>();

app.MapControllers();

app.UseOcelot().Wait();
app.Run();
