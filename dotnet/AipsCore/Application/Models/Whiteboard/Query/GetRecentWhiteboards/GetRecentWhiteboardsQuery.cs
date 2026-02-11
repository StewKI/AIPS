using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Models.Whiteboard.Query.GetRecentWhiteboards;

public record GetRecentWhiteboardsQuery(string UserId): IQuery<ICollection<Infrastructure.Persistence.Whiteboard.Whiteboard>>;