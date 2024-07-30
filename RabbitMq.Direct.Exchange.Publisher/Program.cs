using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory
{
    HostName = "localhost"
};

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

while (true)
{
    Console.Write("Mesaj : ");
    string? message = Console.ReadLine();
    byte[] byteMessage = Encoding.UTF8.GetBytes(message!);

    channel.BasicPublish(
        exchange: "direct-exchange-example",
        routingKey: "direct-exchange-example",
        body: byteMessage);
}