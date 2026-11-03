using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Mappers;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.CreateGroup;

public class CreateGroupCommandHandler(
    ICurrentUserService currentUserService,
    IConversationService conversationService) : IRequestHandler<CreateGroupCommand, ConversationVm>
{
    private long Me => currentUserService.Employee.Id;

    public async Task<ConversationVm> Handle(CreateGroupCommand request, CancellationToken cancellationToken = default)
    {
        return (await conversationService.CreateGroup(request.Title, request.MemberIds, cancellationToken)).ToVm(Me);
    }
}
