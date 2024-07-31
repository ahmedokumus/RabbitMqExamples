using System.Text;
using RabbitMQ.Client;

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

for (int i = 0; i < 100; i++)
{
    await Task.Delay(250);
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
    Console.Write("Topic belirtiniz : ");
    string topic = Console.ReadLine()!;

    channel.BasicPublish(
        exchange: exchange,
        routingKey: topic,
        body: message);
}

Console.Read();