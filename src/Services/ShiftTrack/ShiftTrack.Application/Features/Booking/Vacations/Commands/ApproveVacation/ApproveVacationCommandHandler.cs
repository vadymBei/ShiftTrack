using ShiftTrack.Application.Features.Booking.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Commands.ApproveVacation;

public class ApproveVacationCommandHandler : IRequestHandler<ApproveVacationCommand, VacationVm>
{
    public Task<VacationVm> Handle(ApproveVacationCommand request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}