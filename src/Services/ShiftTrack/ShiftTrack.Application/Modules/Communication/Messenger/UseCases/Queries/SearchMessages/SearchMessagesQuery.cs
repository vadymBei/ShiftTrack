using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.SearchMessages;

public record SearchMessagesQuery(
    long ConversationId,
    string Query) : IRequest<MessageSearchVm>;
