using MassTransit;
using RabbitMq.ESB.MassTransit.Shared.Messages;

const string rabbitMqUri = "rabbitmq://localhost";

const string queueName = "example-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMqUri);
});

ISendEndpoint sendEndpoint = await bus.GetSendEndpoint(new Uri($"{rabbitMqUri}/{queueName}"));

Console.Write("Mesajınızı giriniz : ");
string message = Console.ReadLine()!;

await sendEndpoint.Send<IMessage>(new Message()
{
    Text = message
});

Console.Read();