namespace AipsCore.Application.Common.Message.ErrorMessage;

public interface IErrorMessageHandleStrategy
{
    Task Handle(ErrorMessage message, CancellationToken cancellationToken);
}