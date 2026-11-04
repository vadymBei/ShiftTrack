using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.MarkConversationRead;

public class MarkConversationReadCommandHandler(
    IConversationService conversationService) : IRequestHandler<MarkConversationReadCommand>
{
    public Task Handle(MarkConversationReadCommand request, CancellationToken cancellationToken = default)
    {
        return conversationService.MarkRead(request.Id, request.MessageId, cancellationToken);
    }
}
