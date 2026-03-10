using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Models.Whiteboard.Command.UserCanceledRequestToJoin;

namespace AipsCore.Application.Common.Message.UserCanceledRequestToJoin;

public record UserCanceledRequestToJoinMessage(UserCanceledRequestToJoinCommand Command): IMessage;