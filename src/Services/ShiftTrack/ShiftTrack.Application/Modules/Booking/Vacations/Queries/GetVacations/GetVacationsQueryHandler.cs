using AutoMapper;
using ShiftTrack.Application.Modules.Booking.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Queries.GetVacations;

public class GetVacationsQueryHandler(
    IMapper mapper,
    IVacationService vacationService) :IRequestHandler<GetVacationsQuery, IEnumerable<VacationVm>>
{
    public async Task<IEnumerable<VacationVm>> Handle(GetVacationsQuery request, CancellationToken cancellationToken = default)
    {
       var vacations = await vacationService.GetVacations(request.Filter, cancellationToken);
       
        return mapper.Map<IEnumerable<VacationVm>>(vacations);
    }
}