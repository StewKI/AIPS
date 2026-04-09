using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.Whiteboard.Enums;

namespace AipsCore.Application.Models.Whiteboard.Command.CreateWhiteboard;

public record CreateWhiteboardCommand(
    string Title,
    int MaxParticipants,
    WhiteboardJoinPolicy JoinPolicy)
    : ICommand<CreateWhiteboardCommandResult>;