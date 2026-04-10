using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.Whiteboard.Command.UserCanceledRequestToJoin;

public sealed record UserCanceledRequestToJoinCommand(string WhiteboardId, string UserId): ICommand;