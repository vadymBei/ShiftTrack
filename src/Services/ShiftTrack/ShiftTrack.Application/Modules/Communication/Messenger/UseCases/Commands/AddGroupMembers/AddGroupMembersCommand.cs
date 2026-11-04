using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.AddGroupMembers;

public record AddGroupMembersCommand(
    long Id,
    IReadOnlyCollection<long> EmployeeIds) : IRequest<ConversationVm>;
