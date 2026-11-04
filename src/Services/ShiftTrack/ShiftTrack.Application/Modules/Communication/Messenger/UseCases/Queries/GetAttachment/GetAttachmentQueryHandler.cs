using ShiftTrack.Application.Modules.Communication.Messenger.Constants;
using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.GetAttachment;

public class GetAttachmentQueryHandler(
    IMessageAttachmentService attachmentService) : IRequestHandler<GetAttachmentQuery, AttachmentFileVm>
{
    public async Task<AttachmentFileVm> Handle(GetAttachmentQuery request, CancellationToken cancellationToken = default)
    {
        var file = await attachmentService.Open(request.Id, cancellationToken);

        return new AttachmentFileVm
        {
            Content = file.Content,
            FileName = file.Attachment.FileName,
            ContentType = file.Attachment.ContentType,
            Inline = !request.Download && MessengerConstants.InlineAttachmentTypes.Contains(file.Attachment.ContentType)
        };
    }
}
