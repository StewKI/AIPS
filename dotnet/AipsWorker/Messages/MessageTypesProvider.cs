using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Common.Message.AcceptUserRequestToJoin;
using AipsCore.Application.Common.Message.AddArrow;
using AipsCore.Application.Common.Message.AddLine;
using AipsCore.Application.Common.Message.AddRectangle;
using AipsCore.Application.Common.Message.AddTextShape;
using AipsCore.Application.Common.Message.MoveShape;
using AipsCore.Application.Common.Message.RejectUserRequestToJoin;
using AipsCore.Application.Common.Message.UserCanceledRequestToJoin;

namespace AipsWorker.Messages;

public class MessageTypesProvider : IMessageTypesProvider
{
    public ICollection<Type> GetAllMessageTypes()
    {
        return
        [
            typeof(AddArrowMessage),
            typeof(AddLineMessage),
            typeof(AddRectangleMessage),
            typeof(AddTextShapeMessage),
            typeof(MoveShapeMessage),
            typeof(AcceptUserRequestToJoinMessage),
            typeof(RejectUserRequestToJoinMessage),
            typeof(UserCanceledRequestToJoinMessage)
        ];
    }
}