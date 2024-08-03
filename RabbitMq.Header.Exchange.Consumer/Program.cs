using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory
{
    HostName = "localhost"
};

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

string exchange = "header-exchange-example";

channel.ExchangeDeclare(
    exchange: exchange,
    type: ExchangeType.Headers);

Console.Write("Lütfen header value giriniz : ");
string value = Console.ReadLine()!;

string queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(
    queue: queueName,
    exchange: exchange,
    routingKey: string.Empty,
    new Dictionary<string, object>
    {
        ["no"] = value,
    });

EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();