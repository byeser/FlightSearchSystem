using Booking.Api.Extensions;
using Booking.Application.Commands.CancelBooking;
using Booking.Application.Commands.CreateBooking;
using Booking.Domain.Repositories;
using Booking.Infrastructure.Repositories;
using Consul;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
 
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(CreateBookingCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(CancelBookingCommand).Assembly);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Flight Search API",
        Version = "v1",
        Description = "API for searching flights across multiple providers"
    });
});
builder.Services.AddMetrics();
var app = builder.Build();
app.UseMetricServer();
app.UseHttpMetrics(); 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        };
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(result);
    }
});
app.UseCors("AllowAll");
 
app.UseConsul();
app.UseHttpsRedirection();
app.UseMetricServer();
app.UseHttpMetrics();
 
app.UseAuthorization();
app.MapControllers();
app.Run();
