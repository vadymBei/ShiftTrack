using AutoMapper;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Commands.RejectVacation;

public class RejectVacationCommandHandler(
    IMapper mapper,
    IVacationService vacationService) : IRequestHandler<RejectVacationCommand, VacationVm>
{
    public async Task<VacationVm> Handle(RejectVacationCommand request, CancellationToken cancellationToken = default)
    {
        var vacation = await vacationService.Reject(request.Id, cancellationToken);
        
        return mapper.Map<VacationVm>(vacation);
    }
}