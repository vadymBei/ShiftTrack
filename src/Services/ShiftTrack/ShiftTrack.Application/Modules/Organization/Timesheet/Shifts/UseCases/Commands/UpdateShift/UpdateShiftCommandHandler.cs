using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Commands.UpdateShift;

public class UpdateShiftCommandHandler(
    IMapper mapper,
    IShiftRepository shiftRepository)
    : IRequestHandler<UpdateShiftCommand, ShiftVm>
{
    public async Task<ShiftVm> Handle(UpdateShiftCommand request, CancellationToken cancellationToken)
    {
       var shift = await shiftRepository.Update(request.Data, cancellationToken);

        return mapper.Map<ShiftVm>(shift);
    }
}