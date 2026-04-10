using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.Whiteboard.Command.UnbanUserFromWhiteboard;

public sealed record UnbanUserFromWhiteboardCommand(string UserId, string WhiteboardId) : ICommand;