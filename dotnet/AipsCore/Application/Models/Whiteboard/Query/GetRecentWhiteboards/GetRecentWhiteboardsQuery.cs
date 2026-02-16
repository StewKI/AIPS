using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Models.Whiteboard.Query.GetRecentWhiteboards;

public record GetRecentWhiteboardsQuery: IQuery<ICollection<Infrastructure.Persistence.Whiteboard.Whiteboard>>;