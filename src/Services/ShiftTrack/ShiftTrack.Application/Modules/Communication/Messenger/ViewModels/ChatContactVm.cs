namespace ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;

/// <summary>An employee to start a chat with: only what is needed to choose the right person.</summary>
public class ChatContactVm
{
    public long EmployeeId { get; set; }
    public string FullName { get; set; }
    public string PositionName { get; set; }
    public string DepartmentName { get; set; }
    public string UnitName { get; set; }
    public bool HasPhoto { get; set; }
}
