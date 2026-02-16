using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Models.Whiteboard.Query.GetWhiteboardInfoRT;

public record GetWhiteboardInfoRTQuery(Guid WhiteboardId) : IQuery<Infrastructure.Persistence.Whiteboard.Whiteboard>;