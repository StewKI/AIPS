using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Models.Shape.Command.CreateLine;

namespace AipsCore.Application.Common.Message.AddLine;

public record AddLineMessage(CreateLineCommand Command) : IMessage;