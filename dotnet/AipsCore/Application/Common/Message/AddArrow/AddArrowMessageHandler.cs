using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.MessageBroking;

namespace AipsCore.Application.Common.Message.AddArrow;

public class AddArrowMessageHandler : IMessageHandler<AddArrowMessage>
{
    private readonly IDispatcher _dispatcher;

    public AddArrowMessageHandler(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
    
    public async Task Handle(AddArrowMessage message, CancellationToken cancellationToken)
    {
        await _dispatcher.Execute(message.Command, cancellationToken);
    }
}