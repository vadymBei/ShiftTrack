using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.GetMessages;

public record GetMessagesQuery(
    long ConversationId,
    long? BeforeId,
    int Take) : IRequest<List<MessageVm>>;
