using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.Whiteboard.Command.AcceptUserRequestToJoin;

public sealed record AcceptUserRequestToJoinCommand(string WhiteboardId, string UserId) : ICommand;