using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Models.Whiteboard.Query.GetRecentWhiteboards;

public record GetRecentWhiteboardsQueryResult(ICollection<Infrastructure.Persistence.Whiteboard.Whiteboard> Whiteboards) : IQueryResult;