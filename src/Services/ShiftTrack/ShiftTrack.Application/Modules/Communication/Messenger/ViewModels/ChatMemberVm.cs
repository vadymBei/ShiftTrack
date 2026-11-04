using ShiftTrack.Domain.Modules.Communication.Messenger.Enums;

namespace ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;

public class ChatMemberVm
{
    public long EmployeeId { get; set; }
    public string FullName { get; set; }
    public string PositionName { get; set; }
    public string DepartmentName { get; set; }
    public string UnitName { get; set; }
    public bool HasPhoto { get; set; }
    public ConversationMemberRole Role { get; set; }
    public long? LastReadMessageId { get; set; }
}
