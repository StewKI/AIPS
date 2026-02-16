using System.Collections.Concurrent;
using AipsCore.Application.Abstract;

namespace AipsRT.Model.Whiteboard;

public class WhiteboardManager
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ConcurrentDictionary<Guid, Whiteboard> _whiteboards = new();
    private readonly ConcurrentDictionary<Guid, Guid> _userInWhiteboards = new();
    
    public WhiteboardManager(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task AddWhiteboard(Guid whiteboardId)
    {
        var getWhiteboardService = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<GetWhiteboardService>();
        var whiteboard = await getWhiteboardService.GetWhiteboard(whiteboardId);
        
        _whiteboards[whiteboardId] = whiteboard;
    }

    public bool WhiteboardExists(Guid whiteboardId)
    {
        return _whiteboards.ContainsKey(whiteboardId);
    }

    public void RemoveWhiteboard(Guid whiteboardId)
    {
        _whiteboards.TryRemove(whiteboardId, out _);
    }

    public Whiteboard? GetWhiteboard(Guid whiteboardId)
    {
        return _whiteboards.GetValueOrDefault(whiteboardId);
    }

    public void AddUserToWhiteboard(Guid userId, Guid whiteboardId)
    {
        _userInWhiteboards[userId] = whiteboardId;
    }

    public Guid GetUserWhiteboard(Guid userId)
    {
        return _userInWhiteboards[userId];
    }

    public void RemoveUserFromWhiteboard(Guid userId, Guid whiteboardId)
    {
        _userInWhiteboards.TryRemove(whiteboardId, out _);
    }

    public Whiteboard? GetWhiteboardForUser(Guid userId)
    {
        return GetWhiteboard(GetUserWhiteboard(userId));
    }
}