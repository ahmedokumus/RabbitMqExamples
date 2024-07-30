using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory
{
    HostName = "localhost"
};

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

string queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(
    queue: queueName,
    exchange: "direct-exchange-example",
    routingKey: "direct-exchange-example");

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

// 1. Adım : Publisher'da ki exchange ile birebir aynı isim ve
// type 'a sahip bir exchange tanımlanmalıdır!

// 2. Adım : Publisher tarafından routing key 'de bulunan değerdeki
// kuyruğa gönderilen mesajları, kendi oluşturduğumuz kuyruğa yönlendirerek
// tüketmemiz gerekmektedir. Bunun için öncelikle bir kuyruk oluşturulmalıdır !