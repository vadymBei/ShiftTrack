namespace ShiftTrack.Application.Modules.Booking.Requests.ViewModels;

public class MyBookingRequestsVm
{
    public BookingRequestVm UpcomingAbsence { get; set; }
    public IEnumerable<BookingRequestVm> Items { get; set; }
}
