using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.Whiteboard.Command.AcceptUserRequestToJoin;

public record AcceptUserRequestToJoinCommand(string WhiteboardId, string UserId): ICommand;