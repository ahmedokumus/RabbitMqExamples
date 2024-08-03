using System.Runtime.InteropServices.ComTypes;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory factory = new()
{
    HostName = "localhost"
};

IConnection connection = factory.CreateConnection();
IModel channel = connection.CreateModel();

#region P2P (Point-to-Point) Design

//string queueName = "example-p2p-queue";

//channel.QueueDeclare(
//    queue: queueName,
//    durable: false,
//    exclusive: false,
//    autoDelete: false);

//EventingBasicConsumer consumer = new(channel);

//channel.BasicConsume(
//    queue: queueName,
//    autoAck: false,
//    consumer: consumer);

//consumer.Received += (_, e) =>
//{
//    string message = Encoding.UTF8.GetString(e.Body.Span);
//    Console.WriteLine(message);
//};

#endregion
#region Publish/Subscribe (Pub/Sub) Design

//string exchangeName = "example -pub-sub-exchange";

//channel.ExchangeDeclare(
//    exchange: exchangeName,
//    type: ExchangeType.Fanout);

//string queueName = channel.QueueDeclare().QueueName;

//channel.QueueBind(
//    queue: queueName,
//    exchange: exchangeName,
//    routingKey: string.Empty);

//EventingBasicConsumer consumer = new(channel);

////channel.BasicQos(
////    prefetchCount: 1,
////    prefetchSize: 0,
////    global: false);

//channel.BasicConsume(
//    queue: queueName,
//    autoAck: true,
//    consumer: consumer);

//consumer.Received += (_, e) =>
//{
//    string message = Encoding.UTF8.GetString(e.Body.Span);
//    Console.WriteLine(message);
//};

#endregion
#region Work Queue(İş kuyruğu) Design

//string queueName = "example-work-queue";

//channel.QueueDeclare(
//    queue: queueName,
//    durable: false,
//    exclusive: false,
//    autoDelete: false);

//EventingBasicConsumer consumer = new(channel);

//channel.BasicConsume(
//    queue: queueName,
//    autoAck: true,
//    consumer: consumer);

//channel.BasicQos(
//    prefetchCount: 1,
//    prefetchSize: 0,
//    global: false);

//consumer.Received += (_, e) =>
//{
//    string message = Encoding.UTF8.GetString(e.Body.Span);
//    Console.WriteLine(message);
//};

#endregion

#region Request/Response Design

string requestQueueName = "example-request-response-queue"; //Queue yi biz oluşturduk

channel.QueueDeclare(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: true);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: requestQueueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (_, e) =>
{
    string request = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine($"Request mesajı : {request}");

    string responseMessage = "İşlem Tamamlandı";
    byte[] body = Encoding.UTF8.GetBytes(responseMessage);

    IBasicProperties replyProperties = channel.CreateBasicProperties();
    replyProperties.CorrelationId = e.BasicProperties.CorrelationId;

    channel.BasicPublish(
        exchange: string.Empty,
        //Gelen mesajdaki ReplyTo property'sinde bulunan kuyruk adını alıp,
        //gönderilecek mesajın property'sine veriyoruz.
        routingKey: e.BasicProperties.ReplyTo,
        body: body,
        basicProperties: replyProperties);


};

#endregion

Console.Read();