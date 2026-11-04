using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;

namespace ShiftTrack.Application.Modules.Communication.Messenger.Models;

/// <summary>A stored file to send to the user; the caller disposes <see cref="Content"/>.</summary>
public record AttachmentContent(
    MessageAttachment Attachment,
    Stream Content);
