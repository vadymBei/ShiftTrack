namespace ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;

/// <summary>An employee mentioned in the text as "&lt;@EmployeeId&gt;".</summary>
public class MentionVm
{
    public long EmployeeId { get; set; }
    public string FullName { get; set; }
}
