using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Models.Whiteboard.Query.GetWhiteboardHistory;

public record GetWhiteboardHistoryQuery : IQuery<ICollection<Infrastructure.Persistence.Whiteboard.Whiteboard>>;