using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Models.Shape.Command.CreateLine;

namespace AipsCore.Application.Common.Message.AddLine;

public class AddLineMessageHandler : IMessageHandler<AddLineMessage>
{
    private readonly IDispatcher _dispatcher;

    public AddLineMessageHandler(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public async Task Handle(AddLineMessage message, CancellationToken cancellationToken)
    {
        await _dispatcher.Execute(message.Command, cancellationToken);
    }
}