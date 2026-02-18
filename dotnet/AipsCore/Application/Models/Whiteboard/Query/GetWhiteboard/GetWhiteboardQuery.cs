using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Models.Whiteboard.Query.GetWhiteboard;

public record GetWhiteboardQuery(string WhiteboardId) : IQuery<Infrastructure.Persistence.Whiteboard.Whiteboard?>;