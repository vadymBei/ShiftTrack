using ShiftTrack.Domain.Common.Abstractions;
using ShiftTrack.Domain.Modules.Communication.Messenger.Enums;

namespace ShiftTrack.Domain.Modules.Communication.Messenger.Entities;

/// <summary>A direct chat of two employees or a group chat.</summary>
public class Conversation : AuditableEntity
{
    public long Id { get; set; }

    public ConversationType Type { get; set; }

    /// <summary>Name of a group; direct chats are named after the other member.</summary>
    public string Title { get; set; }

    /// <summary>
    /// "{smaller employee id}:{bigger employee id}" for a direct chat, unique, so a pair of employees
    /// has exactly one direct chat. Null for groups.
    /// </summary>
    public string DirectKey { get; set; }

    public DateTime? LastMessageAt { get; set; }
    public long? LastMessageId { get; set; }

    public List<ConversationMember> Members { get; set; } = [];

    public static string DirectKeyFor(long employeeId, long otherEmployeeId) =>
        employeeId < otherEmployeeId ? $"{employeeId}:{otherEmployeeId}" : $"{otherEmployeeId}:{employeeId}";

    public ConversationMember FindMember(long employeeId) => Members.FirstOrDefault(x => x.EmployeeId == employeeId);

    public bool IsOwner(long employeeId) => FindMember(employeeId)?.Role == ConversationMemberRole.Owner;
}
