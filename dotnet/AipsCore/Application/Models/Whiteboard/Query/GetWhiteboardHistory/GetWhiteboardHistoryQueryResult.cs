using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Models.Whiteboard.Query.GetWhiteboardHistory;

public sealed record GetWhiteboardHistoryQueryResult(ICollection<Infrastructure.Persistence.Whiteboard.Whiteboard> Whiteboards) : IQueryResult;