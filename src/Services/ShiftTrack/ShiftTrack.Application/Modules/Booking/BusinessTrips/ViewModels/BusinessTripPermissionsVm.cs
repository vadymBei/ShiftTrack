namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;

public class BusinessTripPermissionsVm
{
    public bool CanView { get; set; }
    public bool CanEdit { get; set; }
    public bool CanApprove { get; set; }
    public bool CanReject { get; set; }
    public bool CanDelete { get; set; }
    public bool CanDownload { get; set; }
}
