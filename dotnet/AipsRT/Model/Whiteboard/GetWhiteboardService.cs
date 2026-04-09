using AipsCore.Application.Abstract;
using AipsCore.Application.Models.Whiteboard.Query.GetWhiteboardInfoRT;
using AipsCore.Domain.Models.Shape.Enums;
using AipsCore.Domain.Models.WhiteboardMembership.Enums;
using AipsRT.Model.Whiteboard.Shapes.Map;
using AipsRT.Model.Users;

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
        
        var result = await _dispatcher.Execute(query);
        
        return Map(result.Whiteboard);
    }

    private static Whiteboard Map(AipsCore.Infrastructure.Persistence.Whiteboard.Whiteboard entity)
    {
        var whiteboard = new Whiteboard()
        {
            WhiteboardId = entity.Id,
            OwnerId = entity.OwnerId,
            Owner = new User(entity.Owner.Id, entity.Owner.UserName!, entity.Owner.Email!),
            Code = entity.Code,
        };

        foreach (var membership in entity.Memberships)
        {
            whiteboard.AddUser(new User(membership.UserId, membership.User!.UserName!, membership.User.Email!));
        }

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