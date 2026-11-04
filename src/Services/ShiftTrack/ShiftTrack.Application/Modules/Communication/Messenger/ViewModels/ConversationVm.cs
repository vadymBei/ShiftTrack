using ShiftTrack.Domain.Modules.Communication.Messenger.Enums;

namespace ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;

public class ConversationVm
{
    public long Id { get; set; }
    public ConversationType Type { get; set; }

    /// <summary>Name of the group, or the other member's name for a direct chat.</summary>
    public string Title { get; set; }

    /// <summary>The other member of a direct chat; null for groups or when they are no longer available.</summary>
    public ChatMemberVm Interlocutor { get; set; }

    public List<ChatMemberVm> Members { get; set; } = [];

    public MessageVm LastMessage { get; set; }
    public DateTime LastActivityAt { get; set; }

    public int UnreadCount { get; set; }

    /// <summary>The newest message read by any other member: own messages up to it are shown as read (✓✓).</summary>
    public long? ReadByOthersUpToMessageId { get; set; }

    public ConversationMemberRole MyRole { get; set; }

    /// <summary>Muted by the current user: no pop-ups, still counted in the menu.</summary>
    public bool IsMuted { get; set; }

    /// <summary>An unread message mentions the current user.</summary>
    public bool HasUnreadMention { get; set; }

    /// <summary>False when the other member of a direct chat is no longer available.</summary>
    public bool CanWrite { get; set; }
}
