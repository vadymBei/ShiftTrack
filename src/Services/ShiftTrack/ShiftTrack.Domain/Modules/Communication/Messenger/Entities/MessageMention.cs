using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Domain.Modules.Communication.Messenger.Entities;

/// <summary>
/// An employee mentioned in a message. The text keeps the mention as a token "&lt;@id&gt;",
/// so it shows the current name; these rows say whom to notify and whose unread mentions to mark.
/// </summary>
public class MessageMention
{
    public long MessageId { get; set; }
    public Message Message { get; set; }

    public long EmployeeId { get; set; }
    public Employee Employee { get; set; }
}
