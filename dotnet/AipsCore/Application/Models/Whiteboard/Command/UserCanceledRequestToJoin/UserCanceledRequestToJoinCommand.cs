using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.Whiteboard.Command.UserCanceledRequestToJoin;

public record UserCanceledRequestToJoinCommand(string WhiteboardId, string UserId): ICommand;