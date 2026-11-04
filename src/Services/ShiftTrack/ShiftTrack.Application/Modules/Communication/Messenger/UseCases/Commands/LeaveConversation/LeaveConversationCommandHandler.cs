using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.LeaveConversation;

public class LeaveConversationCommandHandler(
    IConversationService conversationService) : IRequestHandler<LeaveConversationCommand>
{
    public Task Handle(LeaveConversationCommand request, CancellationToken cancellationToken = default)
    {
        return conversationService.Leave(request.Id, cancellationToken);
    }
}
