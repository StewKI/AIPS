using AipsCore.Application.Abstract.Query;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership.Enums;
using AipsCore.Domain.Models.WhiteboardMembership.External;
using AipsCore.Domain.Models.WhiteboardMembership.Validation;

namespace AipsCore.Application.Models.Whiteboard.Query.GetMembershipStatus;

public class GetMembershipStatusQueryHandler : IQueryHandler<GetMembershipStatusQuery, GetMembershipStatusQueryResult>
{
    private readonly IWhiteboardMembershipRepository _whiteboardMembershipRepository;

    public GetMembershipStatusQueryHandler(IWhiteboardMembershipRepository whiteboardMembershipRepository)
    {
        _whiteboardMembershipRepository = whiteboardMembershipRepository;
    }

    public async Task<GetMembershipStatusQueryResult> Handle(GetMembershipStatusQuery query, CancellationToken cancellationToken = default)
    {
        var userId = new UserId(query.UserId);
        var whiteboardId = new WhiteboardId(query.WhiteboardId);

        var membership = await _whiteboardMembershipRepository.GetByWhiteboardAndUserAsync(whiteboardId, userId, cancellationToken);

        if (membership is null)
        {
            throw new ValidationException(WhiteboardMembershipErrors.NotFound(whiteboardId, userId));
        }
        
        return new GetMembershipStatusQueryResult(membership.Status);
    }
}