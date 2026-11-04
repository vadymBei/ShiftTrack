using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.GetMessages;

public record GetMessagesQuery(
    long ConversationId,
    long? BeforeId,
    int Take,
    // Reaches back to this message, however many there are before it (up to a limit).
    long? UntilId = null) : IRequest<List<MessageVm>>;
