using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Commands.DeleteVacation;

public class DeleteVacationCommandHandler(
    IVacationService vacationService) : IRequestHandler<DeleteVacationCommand>
{
    public async Task Handle(DeleteVacationCommand request, CancellationToken cancellationToken = default)
    {
        await vacationService.Delete(request.Id, cancellationToken);
    }
}