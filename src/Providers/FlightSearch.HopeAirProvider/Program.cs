using FlightSearch.HopeAirProvider.Services;
using FlightSearch.Shared.Http;
using FlightSearch.Shared.Interfaces;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddRabbitMQ(setup =>
    {
        setup.ConnectionUri = new Uri("amqp://guest:guest@localhost:5672");
    })
        .AddConsul(options =>
        {
            options.HostName = "localhost";
            options.Port = 8500;
        });
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IGenericHttpClient, GenericHttpClient>();
builder.Services.AddScoped<HopeAirProviderService>();
builder.Services.AddScoped<IFlightProvider, HopeAirProviderService>();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HopeAir Provider API",
        Version = "v1"
    });
});

var app = builder.Build();
app.UseMetricServer();
app.UseHttpMetrics();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapGet("/health", () => "Healthy");
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapControllers();

app.Run();