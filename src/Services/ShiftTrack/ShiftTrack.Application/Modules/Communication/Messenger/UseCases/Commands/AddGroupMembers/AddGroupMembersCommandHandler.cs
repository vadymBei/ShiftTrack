using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Mappers;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.AddGroupMembers;

public class AddGroupMembersCommandHandler(
    ICurrentUserService currentUserService,
    IConversationService conversationService) : IRequestHandler<AddGroupMembersCommand, ConversationVm>
{
    private long Me => currentUserService.Employee.Id;

    public async Task<ConversationVm> Handle(AddGroupMembersCommand request, CancellationToken cancellationToken = default)
    {
        return (await conversationService.AddMembers(request.Id, request.EmployeeIds, cancellationToken)).ToVm(Me);
    }
}
