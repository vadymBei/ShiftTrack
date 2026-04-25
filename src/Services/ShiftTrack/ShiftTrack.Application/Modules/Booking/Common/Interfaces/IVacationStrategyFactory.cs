namespace ShiftTrack.Application.Modules.Booking.Common.Interfaces;

public interface IVacationStrategyFactory
{
    IVacationStrategy GetStrategy();
}