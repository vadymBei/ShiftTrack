namespace ShiftTrack.Application.Modules.Communication.Messenger.Models;

/// <summary>A file as it comes from the request.</summary>
public record AttachmentUpload(
    Stream Content,
    string FileName,
    long Length);
