namespace ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;

public interface IVacationShiftService
{
    Task SetVacationShifts(long vacationId, CancellationToken cancellationToken);
}