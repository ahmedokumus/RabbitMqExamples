using System.Text;
using RabbitMQ.Client;

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

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
    Console.Write("Lütfen header value giriniz : ");
    string value = Console.ReadLine()!;

    IBasicProperties basicProperties = channel.CreateBasicProperties();

    basicProperties.Headers = new Dictionary<string, object>
    {
        //["x-match"] = "any", "all" //x-match any ve all olmak üzere 2 değer alır.
        ["no"] = value
    };

    channel.BasicPublish(
        exchange: exchange,
        routingKey: string.Empty,
        body: message,
        basicProperties: basicProperties);
}