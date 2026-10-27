using AutoMapper;
using ShiftTrack.Application.Modules.Booking.Vacations.Extensions;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Commands.ApproveVacation;

public class ApproveVacationCommandHandler(
    IMapper mapper,
    IVacationService vacationService,
    IVacationPermissionService vacationPermissionService) : IRequestHandler<ApproveVacationCommand, VacationVm>
{
    public async Task<VacationVm> Handle(ApproveVacationCommand request, CancellationToken cancellationToken = default)
    {
        var vacation = await vacationService.Approve(request.Id, cancellationToken);

        return vacation.ToVm(mapper, vacationPermissionService);
    }
}