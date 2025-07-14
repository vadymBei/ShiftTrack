using ShiftTrack.Application.Features.Booking.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Queries.GetVacationById;

public class GetVacationByIdQueryHandler : IRequestHandler<GetVacationByIdQuery, VacationVm>
{
    public Task<VacationVm> Handle(GetVacationByIdQuery request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}