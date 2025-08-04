using AutoMapper;
using ShiftTrack.Application.Features.Booking.Common.Interfaces;
using ShiftTrack.Application.Features.Booking.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Commands.RejectVacation;

public class RejectVacationCommandHandler(
    IMapper mapper,
    IVacationService vacationService) : IRequestHandler<RejectVacationCommand, VacationVm>
{
    public async Task<VacationVm> Handle(RejectVacationCommand request, CancellationToken cancellationToken = default)
    {
        var vacation = await vacationService.RejectVacation(request.Id, cancellationToken);
        
        return mapper.Map<VacationVm>(vacation);
    }
}