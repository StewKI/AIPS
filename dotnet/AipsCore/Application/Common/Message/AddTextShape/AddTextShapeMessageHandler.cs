using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.MessageBroking;

namespace AipsCore.Application.Common.Message.AddTextShape;

public class AddTextShapeMessageHandler : IMessageHandler<AddTextShapeMessage>
{
    private readonly IDispatcher _dispatcher;

    public AddTextShapeMessageHandler(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
    
    public async Task Handle(AddTextShapeMessage message, CancellationToken cancellationToken)
    {
        await _dispatcher.Execute(message.Command, cancellationToken);
    }
}