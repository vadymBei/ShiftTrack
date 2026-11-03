using ShiftTrack.Domain.Modules.Communication.Messenger.Enums;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Domain.Modules.Communication.Messenger.Entities;

public class ConversationMember
{
    public long Id { get; set; }

    public long ConversationId { get; set; }
    public Conversation Conversation { get; set; }

    public long EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public ConversationMemberRole Role { get; set; }

    public DateTime JoinedAt { get; set; }

    /// <summary>
    /// Messages up to this one are read. Newer messages of other members are unread, and the other members
    /// see their messages up to this one as read (✓✓).
    /// </summary>
    public long? LastReadMessageId { get; set; }
}
