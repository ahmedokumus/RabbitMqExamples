using MassTransit;
using RabbitMq.ESB.MassTransit.WorkerService.Publisher.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((context, configuration) =>
            {
                configuration.Host("rabbitmq://localhost");
            });
        });

        services.AddHostedService<PublishMessageService>(provider =>
        {
            using IServiceScope scope = provider.CreateScope();
            IPublishEndpoint publishEndpoint = scope.ServiceProvider.GetService<IPublishEndpoint>()!;
            return new PublishMessageService(publishEndpoint);
        });

    })
    .Build();

await host.RunAsync();