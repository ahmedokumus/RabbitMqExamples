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

//byte[] message = Encoding.UTF8.GetBytes("Hello");

//channel.BasicPublish(
//    exchange: string.Empty,
//    routingKey: queueName,
//    body: message);

#endregion
#region Publish/Subscribe (Pub/Sub) Design

//string exchangeName = "example -pub-sub-exchange";

//channel.ExchangeDeclare(
//    exchange: exchangeName,
//    type: ExchangeType.Fanout);

//for (int i = 0;  i < 100; i++)
//{
//    Console.Write("Mesajınızı giriniz : ");
//    string message = Console.ReadLine()!;

//    byte[] body = Encoding.UTF8.GetBytes(message);

//    channel.BasicPublish(
//        exchange: exchangeName,
//        routingKey: string.Empty,
//        body: body);
//}

#endregion
#region Work Queue(İş kuyruğu) Design

//string queueName = "example-work-queue";

//channel.QueueDeclare(
//    queue: queueName,
//    durable: false,
//    exclusive: false,
//    autoDelete: false);

//for (int i = 0; i < 100; i++)
//{
//    Console.Write("Mesajınızı giriniz : ");
//    string message = Console.ReadLine()!;

//    byte[] body = Encoding.UTF8.GetBytes(message);

//    channel.BasicPublish(
//        exchange: string.Empty, 
//        routingKey: queueName,
//        body: body);
//}

#endregion

#region Request/Response Design

string requestQueueName = "example-request-response-queue"; //Queue yi biz oluşturduk

channel.QueueDeclare(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: true);

//Responsun dinleneceği queue tanımlama
string responseQueueName = channel.QueueDeclare().QueueName; //Random oluşturulan queue nin ismini aldık.

//Response sürecinde hangi request'e karşılık respons'un
//yapılacağını ifade edecek olan kolerasyonel değer oluşturma
string correlationId = Guid.NewGuid().ToString();

#region Request gönderme

IBasicProperties properties = channel.CreateBasicProperties();
properties.CorrelationId = correlationId;
properties.ReplyTo = responseQueueName;

Console.Write("Request mesajını giriniz : ");
string message = Console.ReadLine()!;
byte[] body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(
    exchange: string.Empty,
    routingKey: requestQueueName,
    body: body,
    basicProperties: properties);


#endregion

#region Response dinleme

EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(
    queue: responseQueueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (_, e) =>
{
    //Receive edilen mesajın request'te ki kolerasyon değeriyle aynı olup olmadığı
    //kontrol ediliyor ve eğer aynı ise ilgili mesaj response değeri olarak algılanıp
    //işleme tabi tutuluyor.
    if (e.BasicProperties.CorrelationId == correlationId)
    {
        string response = Encoding.UTF8.GetString(e.Body.Span);
        Console.WriteLine($"Response : {response}");
    }
};

#endregion

#endregion

Console.Read();