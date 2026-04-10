using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.Whiteboard.Command.KickUserFromWhiteboard;

public sealed record KickUserFromWhiteboardCommand(string UserId, string WhiteboardId) : ICommand;