using ShiftTrack.Domain.Common.Abstractions;

namespace ShiftTrack.Domain.Modules.Communication.Messenger.Entities;

/// <summary>
/// A file sent in a conversation. It is uploaded before the message is sent and belongs to nobody but
/// the uploader until then (<see cref="MessageId"/> is null); unsent uploads are removed after a day.
/// </summary>
public class MessageAttachment : AuditableEntity
{
    public long Id { get; set; }

    public long ConversationId { get; set; }
    public Conversation Conversation { get; set; }

    public long? MessageId { get; set; }
    public Message Message { get; set; }

    /// <summary>The name the user sees and downloads the file with.</summary>
    public string FileName { get; set; }

    /// <summary>The random name of the file in the storage, so names of users never reach the file system.</summary>
    public string StoredName { get; set; }

    public string ContentType { get; set; }

    /// <summary>Bytes.</summary>
    public long Size { get; set; }

    public bool IsImage => ContentType?.StartsWith("image/", StringComparison.Ordinal) == true;
}
