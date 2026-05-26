using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Commands.CreateShift;

public class CreateShiftCommandHandler(
    IMapper mapper,
    IShiftService shiftService)
    : IRequestHandler<CreateShiftCommand, ShiftVm>
{
    public async Task<ShiftVm> Handle(CreateShiftCommand request, CancellationToken cancellationToken)
    {
        var shift = await shiftService.Create(request.Data, cancellationToken);

        return mapper.Map<ShiftVm>(shift);
    }
}