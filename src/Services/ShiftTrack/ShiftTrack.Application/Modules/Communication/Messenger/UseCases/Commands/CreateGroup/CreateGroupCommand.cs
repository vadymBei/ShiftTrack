using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.CreateGroup;

public record CreateGroupCommand(
    string Title,
    IReadOnlyCollection<long> MemberIds) : IRequest<ConversationVm>;
