using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Models.Whiteboard.External;

public interface IWhiteboardRepository
{
    Task<Whiteboard?> Get(WhiteboardId whiteboardId, CancellationToken cancellationToken = default);
    Task Save(Whiteboard whiteboard, CancellationToken cancellationToken = default);
    Task<bool> WhiteboardCodeExists(WhiteboardCode whiteboardCode);
    
}