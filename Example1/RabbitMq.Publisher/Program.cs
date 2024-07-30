using System.Text;
using RabbitMQ.Client;

//Bağlantı oluşturma 
var factory = new ConnectionFactory
{
    HostName = "localhost"
};

//Bağlantı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Queue Oluşturma
channel.QueueDeclare(queue: "example-queue", exclusive: false);

//Queue'ya mesaj gönderme
////RabbitMq queue ye atacağı mesajları byte türünden kabul etmektedir. Haliyle mesajları bizim byte'a dönüştürmemiz gerekecektir.

//byte[] message = Encoding.UTF8.GetBytes("Merhaba");
//channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(250);
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);
}

Console.Read();