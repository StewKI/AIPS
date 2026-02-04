using AipsCore.Domain.Common.ValueObjects;

namespace AipsCore.Domain.Models.Whiteboard.ValueObjects;

public record WhiteboardId(string IdValue) : DomainId(IdValue);