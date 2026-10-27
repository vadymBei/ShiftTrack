using AutoMapper;
using ShiftTrack.Application.Modules.Booking.Vacations.Extensions;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Queries.GetVacations;

public class GetVacationsQueryHandler(
    IMapper mapper,
    IVacationService vacationService,
    IVacationPermissionService vacationPermissionService) :IRequestHandler<GetVacationsQuery, IEnumerable<VacationVm>>
{
    public async Task<IEnumerable<VacationVm>> Handle(GetVacationsQuery request, CancellationToken cancellationToken = default)
    {
       var vacations = await vacationService.GetFiltered(request.Filter, cancellationToken);
       
        return vacations.Select(x => x.ToVm(mapper, vacationPermissionService)).ToList();
    }
}