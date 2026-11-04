using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.RemoveAttachment;

public record RemoveAttachmentCommand(
    long Id) : IRequest;
