namespace ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;

public interface IVacationStrategyFactory
{
    IVacationStrategy GetStrategy();
}