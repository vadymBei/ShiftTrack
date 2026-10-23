using AutoMapper;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Queries.GetVacationBalanceByEmployeeId;

public class GetVacationBalanceByEmployeeIdQueryHandler(
    IMapper mapper,
    IVacationService vacationService): IRequestHandler<GetVacationBalanceByEmployeeIdQuery, VacationBalanceVm>
{
    public async Task<VacationBalanceVm> Handle(GetVacationBalanceByEmployeeIdQuery request, CancellationToken cancellationToken = default)
    {
        var vacationBalance = await vacationService.GetVacationBalanceByEmployeeId(
            request.EmployeeId,
            cancellationToken);
        
        return mapper.Map<VacationBalanceVm>(vacationBalance);
    }
}