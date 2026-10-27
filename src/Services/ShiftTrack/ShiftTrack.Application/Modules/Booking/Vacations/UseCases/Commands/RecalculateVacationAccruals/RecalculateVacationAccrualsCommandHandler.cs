using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Commands.RecalculateVacationAccruals;

public class RecalculateVacationAccrualsCommandHandler(
    IVacationAccrualService vacationAccrualService) : IRequestHandler<RecalculateVacationAccrualsCommand, int>
{
    public Task<int> Handle(RecalculateVacationAccrualsCommand request, CancellationToken cancellationToken = default)
    {
        return vacationAccrualService.RecalculateAll(cancellationToken);
    }
}
