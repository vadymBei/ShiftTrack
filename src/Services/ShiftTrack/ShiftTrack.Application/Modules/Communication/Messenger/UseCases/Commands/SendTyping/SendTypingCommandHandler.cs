using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.SendTyping;

public class SendTypingCommandHandler(
    IConversationService conversationService) : IRequestHandler<SendTypingCommand>
{
    public Task Handle(SendTypingCommand request, CancellationToken cancellationToken = default)
    {
        return conversationService.Typing(request.Id, cancellationToken);
    }
}
