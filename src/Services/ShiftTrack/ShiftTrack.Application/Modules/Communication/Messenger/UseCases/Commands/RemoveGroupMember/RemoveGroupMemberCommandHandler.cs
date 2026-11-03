using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Mappers;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.RemoveGroupMember;

public class RemoveGroupMemberCommandHandler(
    ICurrentUserService currentUserService,
    IConversationService conversationService) : IRequestHandler<RemoveGroupMemberCommand, ConversationVm>
{
    private long Me => currentUserService.Employee.Id;

    public async Task<ConversationVm> Handle(RemoveGroupMemberCommand request, CancellationToken cancellationToken = default)
    {
        return (await conversationService.RemoveMember(request.Id, request.EmployeeId, cancellationToken)).ToVm(Me);
    }
}
