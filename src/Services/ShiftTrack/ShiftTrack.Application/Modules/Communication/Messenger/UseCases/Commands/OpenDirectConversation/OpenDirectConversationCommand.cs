using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.OpenDirectConversation;

public record OpenDirectConversationCommand(
    long EmployeeId) : IRequest<ConversationVm>;
