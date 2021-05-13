using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Components.Consumers;

namespace Service
{
    class Program
    {
        static void Main(string[] args) => 
            CreateHostBuilder(args).Build().Run();

        static IHostBuilder CreateHostBuilder(string[] args) => 
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddMassTransit(configure => 
                    {
                        configure.AddConsumer<SubmitOrderConsumer>();

                        configure.UsingRabbitMq((busRegistrationContext, rabbitMqBusFactoryConfigurator) =>
                        {
                            rabbitMqBusFactoryConfigurator.ConfigureEndpoints(busRegistrationContext);
                        });
                    });

                    services.AddMassTransitHostedService();
                    
                    services.AddHostedService<Worker>();
                });

    }
}
