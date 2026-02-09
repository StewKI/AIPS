using System.Windows.Input;
using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Application.Models.Whiteboard.Command.CreateWhiteboard;

public record CreateWhiteboardCommand(string OwnerId, string Title) : ICommand<WhiteboardId>;