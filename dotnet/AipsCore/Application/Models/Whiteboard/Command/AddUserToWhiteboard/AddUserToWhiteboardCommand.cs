using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;

namespace AipsCore.Application.Models.Whiteboard.Command.AddUserToWhiteboard;

public record AddUserToWhiteboardCommand(
    string UserId, 
    string WhiteboardId)
    : ICommand;