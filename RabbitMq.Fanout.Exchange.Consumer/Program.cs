using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory
{
    HostName = "localhost"
};

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "fanout-exchange-example",
    type: ExchangeType.Fanout);

Console.Write("Kuyruk adını giriniz : ");
string? queueName = Console.ReadLine();

//Static
channel.QueueDeclare(
    queue: queueName,
    exclusive: false);

channel.QueueBind(
    queue: queueName,
    exchange: "fanout-exchange-example",
    routingKey: string.Empty);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (_, eventArgs) =>
{
    string message = Encoding.UTF8.GetString(eventArgs.Body.Span);
    Console.WriteLine(message);
};

Console.Read();