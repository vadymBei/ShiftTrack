using AutoMapper;
using ShiftTrack.Application.Features.Booking.Common.Interfaces;
using ShiftTrack.Application.Features.Booking.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Commands.ApproveVacation;

public class ApproveVacationCommandHandler(
    IMapper mapper,
    IVacationService vacationService) : IRequestHandler<ApproveVacationCommand, VacationVm>
{
    public async Task<VacationVm> Handle(ApproveVacationCommand request, CancellationToken cancellationToken = default)
    {
        var vacation = await vacationService.ApproveVacation(request.Id, cancellationToken);
        
        return mapper.Map<VacationVm>(vacation);
    }
}