using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Commands.RecalculateVacationAccruals;

/// <summary>Recalculates accrued vacation days of all employees. Returns the number of processed employees.</summary>
public record RecalculateVacationAccrualsCommand : IRequest<int>;
