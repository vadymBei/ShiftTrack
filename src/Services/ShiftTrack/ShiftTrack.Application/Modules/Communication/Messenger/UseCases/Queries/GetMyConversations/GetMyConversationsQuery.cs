using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.GetMyConversations;

public record GetMyConversationsQuery(
    int Take) : IRequest<List<ConversationVm>>;
