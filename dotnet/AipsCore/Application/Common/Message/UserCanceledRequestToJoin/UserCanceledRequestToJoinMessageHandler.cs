using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.MessageBroking;

namespace AipsCore.Application.Common.Message.UserCanceledRequestToJoin;

public class UserCanceledRequestToJoinMessageHandler : IMessageHandler<UserCanceledRequestToJoinMessage>
{
    private readonly IDispatcher _dispatcher;

    public UserCanceledRequestToJoinMessageHandler(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public async Task Handle(UserCanceledRequestToJoinMessage message, CancellationToken cancellationToken)
    {
        await _dispatcher.Execute(message.Command, cancellationToken);
    }
}