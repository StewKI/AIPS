using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Models.Whiteboard.Command.RejectUserRequestToJoin;

namespace AipsCore.Application.Common.Message.RejectUserRequestToJoin;

public record RejectUserRequestToJoinMessage(RejectUserRequestToJoinCommand Command): IMessage;