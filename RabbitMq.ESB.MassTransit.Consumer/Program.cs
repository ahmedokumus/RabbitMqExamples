using MassTransit;
using RabbitMq.ESB.MassTransit.Consumer.Consumers;

const string rabbitMqUri = "rabbitmq://localhost";

const string queueName = "example-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMqUri);
    factory.ReceiveEndpoint(
        queueName:queueName,
        endpoint =>
        {
            endpoint.Consumer<MessageConsumer>();
        });
});

await bus.StartAsync();

Console.Read();