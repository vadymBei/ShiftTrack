using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Queries.DownloadVacationRequestPdf;

public class VacationRequestData
{
    public Vacation Vacation { get; set; }
    public Employee UnitDirector { get; set; }
}