using AutoMapper;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Booking.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Commands.CreateVacation;

public class CreateVacationCommandHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext) : IRequestHandler<CreateVacationCommand, VacationVm>
{
    public Task<VacationVm> Handle(CreateVacationCommand request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}