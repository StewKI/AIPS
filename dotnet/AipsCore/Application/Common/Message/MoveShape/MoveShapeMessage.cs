using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Models.Shape.Command.MoveShape;

namespace AipsCore.Application.Common.Message.MoveShape;

public record MoveShapeMessage(MoveShapeCommand Command) : IMessage;