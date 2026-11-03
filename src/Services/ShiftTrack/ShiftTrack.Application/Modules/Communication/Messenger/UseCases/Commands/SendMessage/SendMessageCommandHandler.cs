using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Mappers;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.SendMessage;

public class SendMessageCommandHandler(
    IMessageService messageService) : IRequestHandler<SendMessageCommand, MessageVm>
{
    public async Task<MessageVm> Handle(SendMessageCommand request, CancellationToken cancellationToken = default)
    {
        return (await messageService.Send(request.ConversationId, request.Text, request.ClientMessageId, request.ReplyToMessageId, cancellationToken)).ToVm();
    }
}
