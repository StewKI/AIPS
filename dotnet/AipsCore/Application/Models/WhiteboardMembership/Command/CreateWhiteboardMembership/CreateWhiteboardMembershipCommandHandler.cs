using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.WhiteboardMembership.External;
using AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;

namespace AipsCore.Application.Models.WhiteboardMembership.Command.CreateWhiteboardMembership;

public class CreateWhiteboardMembershipCommandHandler : ICommandHandler<CreateWhiteboardMembershipCommand, WhiteboardMembershipId>
{
    private readonly IWhiteboardMembershipRepository _whiteboardMembershipRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateWhiteboardMembershipCommandHandler(IWhiteboardMembershipRepository whiteboardMembershipRepository, IUnitOfWork unitOfWork)
    {
        _whiteboardMembershipRepository = whiteboardMembershipRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<WhiteboardMembershipId> Handle(CreateWhiteboardMembershipCommand command, CancellationToken cancellationToken = default)
    {
        var whiteboardMembership = Domain.Models.WhiteboardMembership.WhiteboardMembership.Create(
            command.WhiteboardId, 
            command.UserId, 
            command.IsBanned, 
            command.EditingEnabled, 
            command.CanJoin, 
            command.LastInteractedAt);

        await _whiteboardMembershipRepository.Save(whiteboardMembership, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return whiteboardMembership.Id;
    }
}