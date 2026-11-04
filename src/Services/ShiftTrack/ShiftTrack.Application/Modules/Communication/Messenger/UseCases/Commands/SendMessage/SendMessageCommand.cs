using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.SendMessage;

public record SendMessageCommand(
    long ConversationId,
    string Text,
    Guid? ClientMessageId,
    long? ReplyToMessageId,
    IReadOnlyCollection<long> AttachmentIds) : IRequest<MessageVm>;
