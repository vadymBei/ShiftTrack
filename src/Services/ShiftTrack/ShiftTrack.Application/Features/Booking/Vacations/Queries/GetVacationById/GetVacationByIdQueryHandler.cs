using AutoMapper;
using ShiftTrack.Application.Features.Booking.Common.Interfaces;
using ShiftTrack.Application.Features.Booking.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Queries.GetVacationById;

public class GetVacationByIdQueryHandler(
    IMapper mapper,
    IVacationService vacationService) : IRequestHandler<GetVacationByIdQuery, VacationVm>
{
    public async Task<VacationVm> Handle(GetVacationByIdQuery request, CancellationToken cancellationToken = default)
    {
        var vacation = await vacationService.GetById(request.Id, cancellationToken);
        
        return mapper.Map<VacationVm>(vacation);
    }
}