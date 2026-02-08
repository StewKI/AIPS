using AipsCore.Domain.Common.ValueObjects;

namespace AipsCore.Domain.Models.Shape.ValueObjects;

public record ShapeId(string Value) : DomainId(Value);