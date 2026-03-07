using AipsCore.Application.Abstract.MessageBroking;

namespace AipsCore.Application.Common.Message.ErrorMessage;

public class ErrorMessageHandler : IMessageHandler<ErrorMessage>
{
    private readonly IErrorMessageHandleStrategy _handleStrategy;

    public ErrorMessageHandler(IErrorMessageHandleStrategy handleStrategy)
    {
        _handleStrategy = handleStrategy;
    }
    
    public async Task Handle(ErrorMessage message, CancellationToken cancellationToken)
    {
        await _handleStrategy.Handle(message, cancellationToken);
    }
}