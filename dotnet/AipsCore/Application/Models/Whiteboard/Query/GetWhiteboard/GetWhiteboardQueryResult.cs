using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Models.Whiteboard.Query.GetWhiteboard;

public record GetWhiteboardQueryResult(Infrastructure.Persistence.Whiteboard.Whiteboard? Whiteboard) : IQueryResult;