using AipsCore.Application.Abstract;
using AipsCore.Application.Models.Whiteboard.Query.GetWhiteboardInfoRT;
using AipsCore.Domain.Models.Shape.Enums;
using AipsRT.Model.Whiteboard.Shapes.Map;

namespace AipsRT.Model.Whiteboard;

public class GetWhiteboardService
{
    private readonly IDispatcher _dispatcher;

    public GetWhiteboardService(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public async Task<Whiteboard> GetWhiteboard(Guid whiteboardId)
    {
        var query = new GetWhiteboardInfoRTQuery(whiteboardId);
        return Map(await _dispatcher.Execute(query));
    }

    private static Whiteboard Map(AipsCore.Infrastructure.Persistence.Whiteboard.Whiteboard entity)
    {
        var whiteboard = new Whiteboard()
        {
            WhiteboardId = entity.Id,
            OwnerId = entity.OwnerId,
        };

        foreach (var shape in entity.Shapes)
        {
            switch (shape.Type)
            {
                case ShapeType.Rectangle:
                    whiteboard.AddRectangle(shape.ToRectangle());
                    break;
                case ShapeType.Arrow:
                    whiteboard.AddArrow(shape.ToArrow());
                    break;
                case ShapeType.Line:
                    whiteboard.AddLine(shape.ToLine());
                    break;
                case ShapeType.Text:
                    whiteboard.AddTextShape(shape.ToTextShape());
                    break;
            }
        }

        return whiteboard;
    }
}