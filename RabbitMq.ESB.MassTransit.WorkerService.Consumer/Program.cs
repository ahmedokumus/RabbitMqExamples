using MassTransit;
using RabbitMq.ESB.MassTransit.WorkerService.Consumer.Consumers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(configurator =>
        {
            configurator.AddConsumer<MessageConsumer>();

            configurator.UsingRabbitMq((context, configuration) =>
            {
                configuration.Host("rabbitmq://localhost");

                configuration.ReceiveEndpoint(
                    queueName: "example-message-queue",
                    configureEndpoint: e =>
                    {
                        e.ConfigureConsumer<MessageConsumer>(context);
                    });
            });
        });
    })
    .Build();

await host.RunAsync();