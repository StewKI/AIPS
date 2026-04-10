using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Models.Whiteboard.Query.GetWhiteboardInfoRT;

public sealed record GetWhiteboardInfoRTQuery(Guid WhiteboardId) : IQuery<GetWhiteboardInfoRTQueryResult>;