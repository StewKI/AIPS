using AipsCore.Domain.Models.Shape.External;
using AipsCore.Domain.Models.Shape.ValueObjects;
using AipsCore.Infrastructure.Persistence.Db;
using AipsCore.Infrastructure.Persistence.Shape.Mappers;

namespace AipsCore.Infrastructure.Persistence.Shape;

public class ShapeRepository : IShapeRepository
{
    private readonly AipsDbContext _context;

    public ShapeRepository(AipsDbContext context)
    {
        _context = context;
    }
    
    public async Task<Domain.Models.Shape.Shape?> Get(ShapeId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Shapes.FindAsync([new Guid(id.Value)], cancellationToken);
        
        if (entity is null) return null;
        
        return ShapeMappers.EntityToModel(entity);
    }

    public async Task Add(Domain.Models.Shape.Shape shape, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Shapes.FindAsync([new Guid(shape.Id.Value)], cancellationToken);

        if (entity is not null)
        {
            ShapeMappers.UpdateEntity(entity, shape);
        }
        else
        {
            var newEntity = ShapeMappers.ModelToEntity(shape);
            
            await _context.Shapes.AddAsync(newEntity, cancellationToken);
        }

    }
}