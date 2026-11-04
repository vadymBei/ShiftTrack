using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.RemoveAttachment;

public class RemoveAttachmentCommandHandler(
    IMessageAttachmentService attachmentService) : IRequestHandler<RemoveAttachmentCommand>
{
    public Task Handle(RemoveAttachmentCommand request, CancellationToken cancellationToken = default)
    {
        return attachmentService.RemoveUnsent(request.Id, cancellationToken);
    }
}
