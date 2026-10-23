using ShiftTrack.Application.Modules.Booking.Requests.Enums;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Modules.Booking.Requests.ViewModels;

public class BookingRequestVm
{
    public long Id { get; set; }
    public BookingRequestKind Kind { get; set; }
    public VacationType? VacationType { get; set; }
    public string Route { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
