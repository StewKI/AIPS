using AipsCore.Domain.Models.Whiteboard.Enums;

namespace AipsCore.Domain.Models.Whiteboard;

public partial class Whiteboard
{
    public bool IsPrivate() 
    {
        return JoinPolicy == WhiteboardJoinPolicy.Private;
    }
}