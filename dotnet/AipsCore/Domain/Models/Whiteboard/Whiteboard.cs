using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Models.Whiteboard;

public class Whiteboard
{
    public WhiteboardId Id { get; private set; }
    public UserId WhiteboardOwnerId { get; private set; }
    public WhiteboardCode Code { get; private set; }

    public Whiteboard(WhiteboardId id, User.User whiteboardOwner, WhiteboardCode code)
    {
        Id = id;
        WhiteboardOwnerId = whiteboardOwner.Id;
        Code = code;
    }
}