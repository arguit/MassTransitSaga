using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Components.Consumers;
using Components.StateMachines;

namespace Service
{
    class Program
    {
        static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    services.AddMassTransit(masstransit =>
                    {
                        masstransit.AddConsumer<SubmitOrderConsumer>();

                        masstransit.AddSagaStateMachine<OrderStateMachine, OrderState>()
                            .RedisRepository(redis =>
                            {
                                const string configurationString = "127.0.0.1:5002";

                                redis.DatabaseConfiguration(configurationString);
                            });

                        masstransit.UsingRabbitMq((context, rabbitmq) =>
                        {
                            rabbitmq.ConfigureEndpoints(context);
                        });
                    });

                    services.AddMassTransitHostedService();

                    services.AddHostedService<Worker>();
                });

    }
}
