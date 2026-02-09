using AipsCore.Domain.Models.Shape.ValueObjects;

namespace AipsCore.Domain.Models.Shape.External;

public interface IShapeRepository
{
    Task<Shape?> Get(ShapeId id, CancellationToken cancellationToken = default);
    Task Add(Shape shape, CancellationToken cancellationToken = default);
}