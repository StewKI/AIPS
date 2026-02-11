using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.Shape.ValueObjects;

namespace AipsCore.Domain.Models.Shape.External;

public interface IShapeRepository : IAbstractRepository<Shape, ShapeId>
{
    
}