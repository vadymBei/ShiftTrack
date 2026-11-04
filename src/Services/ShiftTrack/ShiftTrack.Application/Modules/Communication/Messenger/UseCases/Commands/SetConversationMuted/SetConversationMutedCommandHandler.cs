using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Mappers;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.SetConversationMuted;

public class SetConversationMutedCommandHandler(
    ICurrentUserService currentUserService,
    IConversationService conversationService) : IRequestHandler<SetConversationMutedCommand, ConversationVm>
{
    public async Task<ConversationVm> Handle(SetConversationMutedCommand request, CancellationToken cancellationToken = default)
    {
        return (await conversationService.SetMuted(request.Id, request.Muted, cancellationToken))
            .ToVm(currentUserService.Employee.Id);
    }
}
