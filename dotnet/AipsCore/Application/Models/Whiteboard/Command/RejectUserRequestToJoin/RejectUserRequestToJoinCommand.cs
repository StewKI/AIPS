using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.Whiteboard.Command.RejectUserRequestToJoin;

public sealed record RejectUserRequestToJoinCommand(string WhiteboardId, string UserId): ICommand;