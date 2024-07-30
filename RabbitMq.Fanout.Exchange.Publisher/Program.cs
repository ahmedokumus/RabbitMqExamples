using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory
{
    HostName = "localhost"
};

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "fanout-exchange-example",
    type: ExchangeType.Fanout);

while (true)
{
    Console.Write("Mesaj = ");
    string? message = Console.ReadLine();
    byte[] byteMessage = Encoding.UTF8.GetBytes(message!);
    channel.BasicPublish(
        exchange: "fanout-exchange-example",
        routingKey: string.Empty,
        body: byteMessage);
}

//Fanout exchange, bu exchange bind olmuş bütün kuyrukların hepsine isim gözetmeksizin
//bu mesajı iletecekse routing key boş geçilir.