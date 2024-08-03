using MassTransit;
using RabbitMq.ESB.MassTransit.Shared.Messages;

namespace RabbitMq.ESB.MassTransit.WorkerService.Publisher.Services;

public class PublishMessageService : BackgroundService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishMessageService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int i = 0;
        while (true)
        {
            Console.Write("Mesajınızı giriniz : ");
            string userMessage = Console.ReadLine()!;
            Message message = new()
            {
                Text = userMessage
            };

            await _publishEndpoint.Publish(message, stoppingToken);
        }
    }
}