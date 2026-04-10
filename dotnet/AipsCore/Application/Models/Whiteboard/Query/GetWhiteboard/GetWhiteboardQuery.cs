using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Models.Whiteboard.Query.GetWhiteboard;

public sealed record GetWhiteboardQuery(string WhiteboardId) : IQuery<GetWhiteboardQueryResult>;