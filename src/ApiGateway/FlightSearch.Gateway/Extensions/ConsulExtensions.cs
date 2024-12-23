using Consul;

namespace FlightSearch.Gateway.Extensions;


public static class ConsulExtensions
{
    public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConsulClient>(sp => new ConsulClient(cfg =>
        {
            var consulHost = $"http://{configuration["Host"]}:8500";
            cfg.Address = new Uri(consulHost);
        }));

        return services;
    }

    public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
    {
        var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
        var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
        var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();

        var registration = new AgentServiceRegistration()
        {
            ID = configuration["Consul:ServiceId"],
            Name = configuration["Consul:ServiceName"],
            Address = configuration["Consul:ServiceHost"],
            Port = int.Parse(configuration["Consul:ServicePort"]),
            Tags = new[] { "gateway", "api" },
            Check = new AgentServiceCheck()
            {
                HTTP = $"http://{configuration["Consul:ServiceHost"]}:{configuration["Consul:ServicePort"]}/health",
                Interval = TimeSpan.FromSeconds(10)
            }
        };

        consulClient.Agent.ServiceRegister(registration).Wait();

        lifetime.ApplicationStopping.Register(() => {
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
        });

        return app;
    }
} 