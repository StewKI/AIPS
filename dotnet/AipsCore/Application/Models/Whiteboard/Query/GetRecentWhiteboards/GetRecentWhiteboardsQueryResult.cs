using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Models.Whiteboard.Query.GetRecentWhiteboards;

public sealed record GetRecentWhiteboardsQueryResult(ICollection<Infrastructure.Persistence.Whiteboard.Whiteboard> Whiteboards) : IQueryResult;