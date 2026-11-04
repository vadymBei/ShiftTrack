using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.RemoveGroupMember;

public record RemoveGroupMemberCommand(
    long Id,
    long EmployeeId) : IRequest<ConversationVm>;
