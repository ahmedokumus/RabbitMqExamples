namespace RabbitMq.ESB.MassTransit.Shared.Messages;

public class Message : IMessage
{
    public string Text { get; set; }
}