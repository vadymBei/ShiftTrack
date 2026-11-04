using ShiftTrack.Application.Modules.Communication.Messenger.Models;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;

namespace ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;

public interface IMessageAttachmentService
{
    /// <summary>Saves a file of the current user to send in the conversation.</summary>
    Task<MessageAttachment> Upload(long conversationId, AttachmentUpload upload, CancellationToken cancellationToken);

    /// <summary>Removes an upload of the current user that isn't sent yet.</summary>
    Task RemoveUnsent(long id, CancellationToken cancellationToken);

    /// <summary>The file, for the members of its conversation.</summary>
    Task<AttachmentContent> Open(long id, CancellationToken cancellationToken);
}
