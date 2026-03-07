using AipsCore.Application.Abstract.MessageBroking;

namespace AipsCore.Application.Common.Message.TestMessage;

public record TestMessage(string Text) : IMessage;