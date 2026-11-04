using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.SetConversationMuted;

public record SetConversationMutedCommand(
    long Id,
    bool Muted) : IRequest<ConversationVm>;
