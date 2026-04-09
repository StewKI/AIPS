using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Application.Models.Whiteboard.Command.CreateWhiteboard;

public record CreateWhiteboardCommandResult(string WhiteboardId) : ICommandResult;