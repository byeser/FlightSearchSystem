using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSearch.Shared.ServiceDiscovery;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            var address = configuration["Consul:Host"];
            consulConfig.Address = new Uri(address);
        }));

        return services;
    }
}