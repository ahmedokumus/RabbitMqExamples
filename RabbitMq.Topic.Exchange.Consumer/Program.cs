using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory
{
    HostName = "localhost"
};

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

string exchange = "topic-exchange-example";

channel.ExchangeDeclare(
    exchange: exchange,
    type: ExchangeType.Topic);

Console.Write("Topic belirtiniz : ");
string topic = Console.ReadLine()!;

string queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(
    exchange: exchange,
    queue: queueName,
    routingKey: topic);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();