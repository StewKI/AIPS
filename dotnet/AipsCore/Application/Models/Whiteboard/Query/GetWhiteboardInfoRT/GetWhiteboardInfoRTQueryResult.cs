using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Models.Whiteboard.Query.GetWhiteboardInfoRT;

public sealed record GetWhiteboardInfoRTQueryResult(Infrastructure.Persistence.Whiteboard.Whiteboard Whiteboard) : IQueryResult;