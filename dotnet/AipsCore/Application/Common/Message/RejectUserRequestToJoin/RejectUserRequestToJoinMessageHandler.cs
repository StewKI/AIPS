using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.MessageBroking;

namespace AipsCore.Application.Common.Message.RejectUserRequestToJoin;

public class RejectUserRequestToJoinMessageHandler : IMessageHandler<RejectUserRequestToJoinMessage>
{
    private readonly IDispatcher _dispatcher;

    public RejectUserRequestToJoinMessageHandler(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public async Task Handle(RejectUserRequestToJoinMessage message, CancellationToken cancellationToken)
    {
        await _dispatcher.Execute(message.Command, cancellationToken);
    }
}