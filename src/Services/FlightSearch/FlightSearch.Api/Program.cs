using Consul;
using FlightSearch.Api.Extensions;
using FlightSearch.Application.Queries.SearchFlights;
using FlightSearch.AybJetProvider.Services;
using FlightSearch.HopeAirProvider.Services;
using FlightSearch.Shared.Http;
using FlightSearch.Shared.Interfaces;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Prometheus;
using Serilog;

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


builder.Services.AddSingleton<IConsulClient>(sp => new ConsulClient(cfg =>
{
    var serviceConfig = builder.Configuration.GetSection("Consul");
    cfg.Address = new Uri($"http://{serviceConfig["Host"]}:8500");
}));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(SearchFlightsQuery).Assembly);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddHttpClient();
builder.Services.AddScoped<IGenericHttpClient, GenericHttpClient>();
builder.Services.AddScoped<IFlightProvider, HopeAirProviderService>();
builder.Services.AddScoped<IFlightProvider, AybJetProviderService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Flight Search API",
        Version = "v1",
        Description = "API for searching flights across multiple providers"
    });
});
 
builder.Host.UseSerilog((context, config) =>
{
    config.WriteTo.Console()
          .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day);
});

var app = builder.Build();
 
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseMetricServer();
app.UseHttpMetrics(options =>
{
    options.AddCustomLabel("host", context => context.Request.Host.Host);
});
app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();
app.UseConsul();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();