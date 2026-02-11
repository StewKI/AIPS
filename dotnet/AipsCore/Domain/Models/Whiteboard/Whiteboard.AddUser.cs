using System.Runtime.InteropServices.Swift;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.Whiteboard.Enums;
using AipsCore.Domain.Models.Whiteboard.Validation;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership.External;

namespace AipsCore.Domain.Models.Whiteboard;

public partial class Whiteboard : DomainModel<WhiteboardId>
{
    public async Task AddUserAsync(
        User.User user, 
        IWhiteboardMembershipRepository membershipRepository, 
        CancellationToken cancellationToken = default)
    {
        var membership 
            = await membershipRepository.GetByWhiteboardAndUserAsync(this.Id, user.Id, cancellationToken);

        if (membership is not null)
        {
            throw new ValidationException(WhiteboardErrors.UserAlreadyAdded(user.Id));
        }

        membership = WhiteboardMembership.WhiteboardMembership.Create(
            this.Id.ToString(),
            user.Id.ToString(),
            false,
            false,
            this.GetCanJoin(),
            DateTime.Now
        );
        
        await membershipRepository.AddAsync(membership, cancellationToken);
    }

    private bool GetCanJoin()
    {
        return this.JoinPolicy == WhiteboardJoinPolicy.FreeToJoin;
    }
}
