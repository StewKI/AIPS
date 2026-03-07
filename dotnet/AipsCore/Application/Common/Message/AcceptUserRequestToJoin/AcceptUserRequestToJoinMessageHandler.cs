using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.MessageBroking;

namespace AipsCore.Application.Common.Message.AcceptUserRequestToJoin;

public class AcceptUserRequestToJoinMessageHandler : IMessageHandler<AcceptUserRequestToJoinMessage>
{
    private readonly IDispatcher _dispatcher;

    public AcceptUserRequestToJoinMessageHandler(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public async Task Handle(AcceptUserRequestToJoinMessage message, CancellationToken cancellationToken)
    {
        await _dispatcher.Execute(message.Command, cancellationToken);
    }
}