using AipsCore.Application.Abstract.MessageBroking;

namespace AipsCore.Application.Common.Message.TestMessage;

public class TestMessageHandler : IMessageHandler<TestMessage>
{

    public Task Handle(TestMessage message, CancellationToken cancellationToken)
    {
        Console.WriteLine(message.Text);
        
        return Task.CompletedTask;
    }
}