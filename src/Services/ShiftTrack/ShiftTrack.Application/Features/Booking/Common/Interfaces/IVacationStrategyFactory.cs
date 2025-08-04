namespace ShiftTrack.Application.Features.Booking.Common.Interfaces;

public interface IVacationStrategyFactory
{
    IVacationStrategy GetStrategy();
}