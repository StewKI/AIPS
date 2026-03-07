using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Models.Whiteboard.Command.AcceptUserRequestToJoin;

namespace AipsCore.Application.Common.Message.AcceptUserRequestToJoin;

public record AcceptUserRequestToJoinMessage(AcceptUserRequestToJoinCommand Command) : IMessage;