using FlightSearch.Gateway.Startup;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Prometheus;
using FlightSearch.Gateway.Extensions;
using Consul;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

 

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddConsul(options =>
    {
        options.HostName = "localhost";
        options.Port = 8500; // Consul port
    })
    .AddRabbitMQ(
        rabbitConnectionString: "amqp://guest:guest@localhost:5672",
        failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
        tags: new[] { "rabbitmq", "message-broker" },
        timeout: TimeSpan.FromSeconds(5));





builder.Services.AddSingleton<IConsulClient>(sp => new ConsulClient(cfg =>
{
    var serviceConfig = builder.Configuration.GetSection("Consul");
    cfg.Address = new Uri($"http://{serviceConfig["Host"]}:8500");
}));



builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Flight Search Gateway API",
        Description = "Gateway API for Flight Search System"
    });
     
    options.DocumentFilter<SwaggerEndpointFilter>();
});
 
builder.Services.AddHttpClient();  
builder.Services.AddMetrics();
var app = builder.Build(); 
app.UseMetricServer(); 
app.UseHttpMetrics(options =>
{
    options.AddCustomLabel("gateway_name", context => "main_gateway");
});

// Gateway-specific metrics
var gatewayRequests = Prometheus.Metrics.CreateCounter(
    "gateway_requests_total",
    "Total requests handled by the gateway",
    new CounterConfiguration
    {
        LabelNames = new[] { "path", "method", "status_code" }
    }
);

var gatewayLatency = Prometheus.Metrics.CreateHistogram(
    "gateway_request_duration_seconds",
    "Request duration in seconds for gateway",
    new HistogramConfiguration
    {
        LabelNames = new[] { "path", "method" }
    }
);

// Middleware ekleyelim ki metrikleri toplayabilelim
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value;
    var method = context.Request.Method;

    using (gatewayLatency.WithLabels(path, method).NewTimer())
    {
        try
        {
            await next();

            gatewayRequests.WithLabels(path, method, context.Response.StatusCode.ToString()).Inc();
        }
        catch
        {
            gatewayRequests.WithLabels(path, method, "500").Inc();
            throw;
        }
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseConsul();
app.UseRouting();
app.UseHttpsRedirection();

app.MapReverseProxy();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
 
app.UseHttpMetrics(options =>
{
    options.AddCustomLabel("host", context => context.Request.Host.Host);
});
app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();
app.UseConsul();
app.Run();