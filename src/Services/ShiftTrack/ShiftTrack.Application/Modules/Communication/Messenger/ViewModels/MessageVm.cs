using ShiftTrack.Domain.Modules.Communication.Messenger.Enums;

namespace ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;

public class MessageVm
{
    public long Id { get; set; }
    public long ConversationId { get; set; }
    public MessageType Type { get; set; }

    /// <summary>Null for a deleted message.</summary>
    public string Text { get; set; }

    public long? AuthorId { get; set; }
    public string AuthorFullName { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public bool IsDeleted { get; set; }

    /// <summary>Lets the sender match the saved message to the one shown before the response.</summary>
    public Guid? ClientMessageId { get; set; }

    public MessagePreviewVm ReplyTo { get; set; }

    /// <summary>Empty for a deleted message.</summary>
    public List<AttachmentVm> Attachments { get; set; } = [];

    /// <summary>Names for the "&lt;@id&gt;" tokens in <see cref="Text"/>.</summary>
    public List<MentionVm> Mentions { get; set; } = [];
}
