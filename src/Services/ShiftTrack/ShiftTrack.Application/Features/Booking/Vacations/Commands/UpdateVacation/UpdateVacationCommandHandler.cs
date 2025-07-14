using ShiftTrack.Application.Features.Booking.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Commands.UpdateVacation;

public class UpdateVacationCommandHandler : IRequestHandler<UpdateVacationCommand, VacationVm>
{
    public Task<VacationVm> Handle(UpdateVacationCommand request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}