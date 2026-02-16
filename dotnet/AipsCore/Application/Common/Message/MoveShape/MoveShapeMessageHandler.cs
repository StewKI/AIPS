using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.MessageBroking;

namespace AipsCore.Application.Common.Message.MoveShape;

public class MoveShapeMessageHandler : IMessageHandler<MoveShapeMessage>
{
    private readonly IDispatcher _dispatcher;

    public MoveShapeMessageHandler(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
    
    public async Task Handle(MoveShapeMessage message, CancellationToken cancellationToken)
    {
        await _dispatcher.Execute(message.Command, cancellationToken);
    }
}