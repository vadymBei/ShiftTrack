namespace ShiftTrack.Application.Modules.Organization.Employees.ViewModels;

/// <summary>An employee who has a photo; the version changes when the photo does.</summary>
public class EmployeePhotoVm
{
    public long EmployeeId { get; set; }
    public string Version { get; set; }
}
