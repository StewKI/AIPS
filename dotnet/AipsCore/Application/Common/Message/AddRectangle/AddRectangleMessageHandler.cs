using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.MessageBroking;

namespace AipsCore.Application.Common.Message.AddRectangle;

public class AddRectangleMessageHandler : IMessageHandler<AddRectangleMessage>
{
    private readonly IDispatcher _dispatcher;

    public AddRectangleMessageHandler(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
    
    public async Task Handle(AddRectangleMessage message, CancellationToken cancellationToken)
    {
        await _dispatcher.Execute(message.Command, cancellationToken);
    }
}