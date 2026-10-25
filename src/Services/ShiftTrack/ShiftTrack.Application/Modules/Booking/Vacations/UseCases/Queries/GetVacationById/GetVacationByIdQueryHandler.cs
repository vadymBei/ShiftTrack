using AutoMapper;
using ShiftTrack.Application.Modules.Booking.Vacations.Extensions;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Queries.GetVacationById;

public class GetVacationByIdQueryHandler(
    IMapper mapper,
    IVacationService vacationService,
    IVacationPermissionService vacationPermissionService) : IRequestHandler<GetVacationByIdQuery, VacationVm>
{
    public async Task<VacationVm> Handle(GetVacationByIdQuery request, CancellationToken cancellationToken = default)
    {
        var vacation = await vacationService.GetById(request.Id, cancellationToken);
        
        return vacation.ToVm(mapper, vacationPermissionService);
    }
}