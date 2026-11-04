using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Mappers;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.UploadAttachment;

public class UploadAttachmentCommandHandler(
    IMessageAttachmentService attachmentService) : IRequestHandler<UploadAttachmentCommand, AttachmentVm>
{
    public async Task<AttachmentVm> Handle(UploadAttachmentCommand request, CancellationToken cancellationToken = default)
    {
        return (await attachmentService.Upload(request.ConversationId, request.File, cancellationToken)).ToVm();
    }
}
