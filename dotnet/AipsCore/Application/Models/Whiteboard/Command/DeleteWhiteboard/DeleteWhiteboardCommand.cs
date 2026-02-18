using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.Whiteboard.Command.DeleteWhiteboard;

public record DeleteWhiteboardCommand(string WhiteboardId) : ICommand;