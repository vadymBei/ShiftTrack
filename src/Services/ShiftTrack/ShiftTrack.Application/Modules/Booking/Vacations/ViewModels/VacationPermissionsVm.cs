namespace ShiftTrack.Application.Modules.Booking.Vacations.ViewModels;

public class VacationPermissionsVm
{
    public bool CanView { get; set; }
    public bool CanEdit { get; set; }
    public bool CanApprove { get; set; }
    public bool CanReject { get; set; }
    public bool CanDelete { get; set; }
    public bool CanDownload { get; set; }

    /// <summary>Approve moves the author's draft to "pending approval" instead of approving it.</summary>
    public bool ApproveSubmitsForApproval { get; set; }
}
