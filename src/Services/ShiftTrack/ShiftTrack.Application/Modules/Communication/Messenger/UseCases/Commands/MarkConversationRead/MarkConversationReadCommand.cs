using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.MarkConversationRead;

public record MarkConversationReadCommand(
    long Id,
    long MessageId) : IRequest;
