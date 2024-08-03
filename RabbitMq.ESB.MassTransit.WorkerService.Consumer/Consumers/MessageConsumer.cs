using MassTransit;
using RabbitMq.ESB.MassTransit.Shared.Messages;

namespace RabbitMq.ESB.MassTransit.WorkerService.Consumer.Consumers;

public class MessageConsumer : IConsumer<IMessage>
{
    public Task Consume(ConsumeContext<IMessage> context)
    {
        Console.WriteLine($"Gelen mesaj : {context.Message.Text}");
        return Task.CompletedTask;
    }
}