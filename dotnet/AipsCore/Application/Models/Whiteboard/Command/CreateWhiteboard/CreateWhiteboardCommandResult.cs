using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Application.Models.Whiteboard.Command.CreateWhiteboard;

public sealed record CreateWhiteboardCommandResult(string WhiteboardId) : ICommandResult;