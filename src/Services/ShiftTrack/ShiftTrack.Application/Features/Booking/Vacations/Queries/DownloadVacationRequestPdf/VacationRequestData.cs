using ShiftTrack.Domain.Features.Booking.Vacations.Entities;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;

namespace ShiftTrack.Application.Features.Booking.Vacations.Queries.DownloadVacationRequestPdf;

public class VacationRequestData
{
    public Vacation Vacation { get; set; }
    public Employee UnitDirector { get; set; }
}