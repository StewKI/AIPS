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

    protected override Domain.Models.Shape.Shape MapToDomainEntity(Shape persistenceEntity)
    {
        return ShapeMappers.MapToDomainEntity(persistenceEntity);
    }

    protected override Shape MapToPersistenceEntity(Domain.Models.Shape.Shape domainEntity)
    {
        return ShapeMappers.MapToPersistenceEntity(domainEntity);
    }

    protected override void UpdatePersistenceEntity(Shape persistenceEntity, Domain.Models.Shape.Shape domainEntity)
    {
        ShapeMappers.UpdatePersistenceEntity(persistenceEntity, domainEntity);
    }
}