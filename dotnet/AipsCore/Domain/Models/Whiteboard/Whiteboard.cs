using AipsCore.Domain.Abstract;
using AipsCore.Domain.Common.ValueObjects;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.Enums;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Models.Whiteboard;

public partial class Whiteboard : DomainModel<WhiteboardId>
{
    public UserId WhiteboardOwnerId { get; private set; }
    public WhiteboardCode Code { get; private set; }
    public WhiteboardTitle Title { get; private set; }
    public CreatedAt CreatedAt { get; private set; }
    public DeletedAt DeletedAt { get; private set; }
    public WhiteboardMaxParticipants MaxParticipants { get; private set; }
    public WhiteboardJoinPolicy JoinPolicy { get; private set; }
    public WhiteboardState State { get; private set; }

    public Whiteboard(
        WhiteboardId id, 
        User.User whiteboardOwner, 
        WhiteboardCode code, 
        WhiteboardTitle title,
        CreatedAt createdAt, 
        DeletedAt deletedAt, 
        WhiteboardMaxParticipants maxParticipants,
        WhiteboardJoinPolicy joinPolicy,
        WhiteboardState state)
        : base(id)
    {
        WhiteboardOwnerId = whiteboardOwner.Id;
        Code = code;
        Title = title;
        CreatedAt = createdAt;
        DeletedAt = deletedAt;
        MaxParticipants = maxParticipants;
        JoinPolicy = joinPolicy;
        State = state;
    }

    public Whiteboard(
        WhiteboardId id, 
        UserId whiteboardOwnerId,
        WhiteboardCode code, 
        WhiteboardTitle title, 
        CreatedAt createdAt,
        DeletedAt deletedAt, 
        WhiteboardMaxParticipants maxParticipants,
        WhiteboardJoinPolicy joinPolicy,
        WhiteboardState state)
        : base(id)
    {
        WhiteboardOwnerId = whiteboardOwnerId;
        Code = code;
        Title = title;
        CreatedAt = createdAt;
        DeletedAt = deletedAt;
        MaxParticipants = maxParticipants;
        JoinPolicy = joinPolicy;
        State = state;
    }
    
    public static Whiteboard Create(
        string id, 
        string ownerId,
        string code,
        string title,
        DateTime createdAt,
        DateTime? deletedAt,
        int maxParticipants,
        WhiteboardJoinPolicy joinPolicy,
        WhiteboardState state)
    {
        var whiteboardId = new WhiteboardId(id);
        var whiteboardOwnerId = new UserId(ownerId);
        var whiteboardCode = new WhiteboardCode(code);
        var whiteboardTitle = new WhiteboardTitle(title);
        var whiteboardCreatedAt = new CreatedAt(createdAt);
        var whiteboardDeletedAt = new DeletedAt(deletedAt);
        var whiteboardMaxParticipants = new WhiteboardMaxParticipants(maxParticipants);
        
        return new Whiteboard(
            whiteboardId, 
            whiteboardOwnerId, 
            whiteboardCode, 
            whiteboardTitle,
            whiteboardCreatedAt,
            whiteboardDeletedAt,
            whiteboardMaxParticipants,
            joinPolicy,
            state);
    }

    public static Whiteboard Create(
        string ownerId,
        string code,
        string title,
        int maxParticipants,
        WhiteboardJoinPolicy joinPolicy)
    {
        var whiteboardId = WhiteboardId.Any();
        var whiteboardOwnerId = new UserId(ownerId);
        var whiteboardCode = new WhiteboardCode(code);
        var whiteboardTitle = new WhiteboardTitle(title);
        var whiteboardCreatedAt = new CreatedAt(DateTime.UtcNow);
        var whiteboardDeletedAt = new DeletedAt(null);
        var whiteboardMaxParticipants = new WhiteboardMaxParticipants(maxParticipants);
        
        return new Whiteboard(
            whiteboardId, 
            whiteboardOwnerId, 
            whiteboardCode, 
            whiteboardTitle,
            whiteboardCreatedAt,
            whiteboardDeletedAt,
            whiteboardMaxParticipants,
            joinPolicy,
            WhiteboardState.Active);
    }
}