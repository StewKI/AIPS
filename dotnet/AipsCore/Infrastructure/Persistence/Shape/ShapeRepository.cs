using AipsCore.Domain.Models.Shape.External;
using AipsCore.Domain.Models.Shape.ValueObjects;
using AipsCore.Infrastructure.Persistence.Abstract;
using AipsCore.Infrastructure.Persistence.Db;
using AipsCore.Infrastructure.Persistence.Shape.Mappers;

namespace AipsCore.Infrastructure.Persistence.Shape;

public class ShapeRepository : AbstractRepository<Domain.Models.Shape.Shape, ShapeId, Shape>, IShapeRepository
{
    public ShapeRepository(AipsDbContext context) 
        : base(context)
    {
        
    }

    protected override Domain.Models.Shape.Shape MapToModel(Shape entity)
    {
        return ShapeMappers.MapToEntity(entity);
    }

    protected override Shape MapToEntity(Domain.Models.Shape.Shape model)
    {
        return ShapeMappers.MapToEntity(model);
    }

    protected override void UpdateEntity(Shape entity, Domain.Models.Shape.Shape model)
    {
        ShapeMappers.UpdateEntity(entity, model);
    }
}