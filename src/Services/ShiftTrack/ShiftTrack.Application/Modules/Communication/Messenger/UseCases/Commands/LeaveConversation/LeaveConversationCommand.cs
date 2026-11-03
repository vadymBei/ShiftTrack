using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.LeaveConversation;

public record LeaveConversationCommand(
    long Id) : IRequest;
