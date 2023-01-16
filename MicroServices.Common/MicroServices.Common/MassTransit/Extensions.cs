using MassTransit;
using MassTransit.Definition;
using MicroServices.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MicroServices.Common.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services)
        {
            services.AddMassTransit(configure=> 
            {
                configure.AddConsumers(Assembly.GetEntryAssembly());
                configure.UsingRabbitMq((context,configurator) => 
                {
                    var service = context.GetService<IConfiguration>();
                    var rabbitMQSettings = service?.GetSection("RabbitMQSettings").Get<RabbitMQSettings>();
                    var serviceSettings = service?.GetSection("ServiceSettings").Get<ServiceSettings>();
                    configurator.Host(rabbitMQSettings?.Host);
                    configurator.ConfigureEndpoints(context, 
                        new KebabCaseEndpointNameFormatter(serviceSettings?.ServiceName,
                        includeNamespace: false));
                });
            });
            services.AddMassTransitHostedService();
            return services;
        }
    }
}
