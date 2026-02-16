using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Models.Shape.Command.CreateTextShape;

namespace AipsCore.Application.Common.Message.AddTextShape;

public record AddTextShapeMessage(CreateTextShapeCommand Command) : IMessage;