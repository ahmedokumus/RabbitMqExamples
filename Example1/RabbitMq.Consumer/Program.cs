using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//Bağlantı oluşturma 
var factory = new ConnectionFactory();
factory.HostName = "localhost";

//Bağlantı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Queue oluşturma
channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true);//Consumer'daki kuyruk publisher'daki kuyruk ile birebir aynı olmalıdır

//Queue'dan mesaj okuma
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "example-queue", autoAck:false, consumer);
consumer.Received += (sender, e) =>
{
    //Kuyruğa gelen mesajın işlendiği yerdir.
    //e.Body : Kuyruktaki mesajın verisini bütünsel olarak getirecektir
    //e.Body.Span veya e.Body.ToArray() : Kuyruktaki mesajın byte verisini getirecektir.
    var message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
    channel.BasicAck(e.DeliveryTag, multiple: false);
};

Console.Read();