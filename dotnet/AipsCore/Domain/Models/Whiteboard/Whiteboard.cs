using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Models.Whiteboard;

public class Whiteboard
{
    public WhiteboardId Id { get; private set; }
    public UserId WhiteboardOwnerId { get; private set; }
    public WhiteboardCode Code { get; private set; }
    public WhiteboardTitle Title { get; private set; }

    public Whiteboard(WhiteboardId id, User.User whiteboardOwner, WhiteboardCode code, WhiteboardTitle title)
    {
        Id = id;
        WhiteboardOwnerId = whiteboardOwner.Id;
        Code = code;
        Title = title;
    }

    public Whiteboard(WhiteboardId id, UserId whiteboardOwnerId, WhiteboardCode code, WhiteboardTitle title)
    {
        Id = id;
        WhiteboardOwnerId = whiteboardOwnerId;
        Code = code;
        Title = title;    
    }
    
    public static Whiteboard Create(string id, string ownerId, string code, string title)
    {
        var whiteboardId = new WhiteboardId(id);
        var whiteboardOwnerId = new UserId(ownerId);
        var whiteboardCode = new WhiteboardCode(code);
        var whiteboardTitle = new WhiteboardTitle(title);
        
        return new Whiteboard(whiteboardId, whiteboardOwnerId, whiteboardCode, whiteboardTitle);
    }

    public static Whiteboard Create(string ownerId, string code, string title)
    {
        var whiteboardId = WhiteboardId.Any();
        var whiteboardOwnerId = new UserId(ownerId);
        var whiteboardCode = new WhiteboardCode(code);
        var whiteboardTitle = new WhiteboardTitle(title);
        
        return new Whiteboard(whiteboardId, whiteboardOwnerId, whiteboardCode, whiteboardTitle);
    }
}