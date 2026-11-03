using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;

namespace ShiftTrack.Application.Modules.Communication.Messenger.Models;

/// <summary>A conversation as one member sees it in the list.</summary>
public class ConversationSummary
{
    /// <summary>With members and their employees, position, department and unit.</summary>
    public Conversation Conversation { get; init; }

    public Message LastMessage { get; init; }

    /// <summary>Messages of other members after the last read one.</summary>
    public int UnreadCount { get; init; }
}
