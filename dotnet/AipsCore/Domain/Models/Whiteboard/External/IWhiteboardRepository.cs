using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Models.Whiteboard.External;

public interface IWhiteboardRepository 
    : IAbstractRepository<Whiteboard, WhiteboardId>, ISoftDeletableRepository<WhiteboardId>
{
    Task<bool> WhiteboardCodeExists(WhiteboardCode whiteboardCode);
}