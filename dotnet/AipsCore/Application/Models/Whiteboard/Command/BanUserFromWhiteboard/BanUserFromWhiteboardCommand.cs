using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.Whiteboard.Command.BanUserFromWhiteboard;

public record BanUserFromWhiteboardCommand(string CallerId, string UserId, string WhiteboardId) : ICommand;