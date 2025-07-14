using ShiftTrack.Application.Features.Booking.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Commands.RejectVacation;

public class RejectVacationCommandHandler : IRequestHandler<RejectVacationCommand, VacationVm>
{
    public Task<VacationVm> Handle(RejectVacationCommand request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}