using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.WhiteboardMembership.External;
using AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;

namespace AipsCore.Application.Models.WhiteboardMembership.Command.CreateWhiteboardMembership;

public class CreateWhiteboardMembershipCommandHandler : ICommandHandler<CreateWhiteboardMembershipCommand, WhiteboardMembershipId>
{
    private readonly IWhiteboardMembershipRepository _whiteboardMembershipRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateWhiteboardMembershipCommandHandler(
        IWhiteboardMembershipRepository whiteboardMembershipRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _whiteboardMembershipRepository = whiteboardMembershipRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<WhiteboardMembershipId> Handle(CreateWhiteboardMembershipCommand command, CancellationToken cancellationToken = default)
    {
        var userId = _userContext.GetCurrentUserId();
        
        var whiteboardMembership = Domain.Models.WhiteboardMembership.WhiteboardMembership.Create(
            command.WhiteboardId, 
            userId.IdValue, 
            command.IsBanned, 
            command.EditingEnabled, 
            command.CanJoin, 
            DateTime.UtcNow);

        await _whiteboardMembershipRepository.SaveAsync(whiteboardMembership, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return whiteboardMembership.Id;
    }
}